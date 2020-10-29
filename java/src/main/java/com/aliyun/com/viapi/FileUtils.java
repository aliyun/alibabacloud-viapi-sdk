package com.aliyun.com.viapi;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.URL;
import java.net.URLConnection;
import java.net.URLDecoder;
import java.util.HashMap;
import java.util.Map;
import java.util.UUID;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import com.aliyun.oss.OSSClient;
import com.aliyuncs.DefaultAcsClient;
import com.aliyuncs.exceptions.ClientException;
import com.aliyuncs.profile.DefaultProfile;
import com.aliyuncs.profile.IClientProfile;
import com.aliyuncs.viapiutils.model.v20200401.GetOssStsTokenRequest;
import com.aliyuncs.viapiutils.model.v20200401.GetOssStsTokenResponse;
import org.apache.commons.lang3.StringUtils;

public class FileUtils {

    DefaultAcsClient client;
    String endpoint = "viapiutils.cn-shanghai.aliyuncs.com";
    String accessKeyId;
    Pattern fileNameRegex =
        Pattern.compile("\\w+.(jpg|gif|png|jpeg|bmp|mov|mp4|avi)");


    static Map<String, FileUtils> map = new HashMap<String, FileUtils>();

    public static synchronized  FileUtils getInstance(String accessKeyId, String accessKeySecret)
        throws ClientException {
        String mapKey = accessKeyId + accessKeySecret;
        FileUtils fileUtils = map.get(mapKey);
        if(fileUtils == null){
            fileUtils = new FileUtils(accessKeyId , accessKeySecret);
            map.put(mapKey , fileUtils);
        }
        return fileUtils;
    }


    private FileUtils(String accessKeyId, String accessKeySecret) throws ClientException {
        IClientProfile profile = DefaultProfile.getProfile("cn-shanghai", accessKeyId, accessKeySecret);
        DefaultProfile.addEndpoint("", "cn-shanghai", "viapiutils", endpoint);

        client = new DefaultAcsClient(profile);

        this.accessKeyId = accessKeyId;

    }

    public String upload(String filePath) throws ClientException, IOException {

        InputStream ins = null;
        try {
            String fileName = "";

            if (StringUtils.startsWithAny(filePath, "http://", "https://")) {
                filePath = URLDecoder.decode(filePath,"UTF-8");

                Matcher matcher = fileNameRegex.matcher(StringUtils.lowerCase(filePath));
                if(matcher.find()){
                    fileName = matcher.group();
                }

                URL url = new URL(filePath);
                if(StringUtils.isBlank(fileName)) {
                    fileName = url.getPath();
                    fileName = StringUtils.substringAfterLast(fileName, "/");
                }

                URLConnection urlConnection = url.openConnection();
                ins = urlConnection.getInputStream();
            } else {
                File file = new File(filePath);
                fileName = file.getName();
                ins = new FileInputStream(file);
            }

            return upload(fileName, ins);
        }finally {
            if(ins != null){
                ins.close();
            }
        }

    }

    public String upload(String fileName, InputStream stream) throws ClientException, IOException {

        GetOssStsTokenRequest getOssStsTokenRequest = new GetOssStsTokenRequest();
        GetOssStsTokenResponse getOssStsTokenResponse = client.getAcsResponse(getOssStsTokenRequest);
        String akKey = getOssStsTokenResponse.getData().getAccessKeyId();
        String akSec = getOssStsTokenResponse.getData().getAccessKeySecret();
        String token = getOssStsTokenResponse.getData().getSecurityToken();
        OSSClient ossClient = new OSSClient("http://oss-cn-shanghai.aliyuncs.com", akKey, akSec, token);
        String key = accessKeyId + "/" + UUID.randomUUID().toString() + fileName;

        ossClient.putObject("viapi-customer-temp", key, stream);
        return "http://viapi-customer-temp.oss-cn-shanghai.aliyuncs.com/" + key ;

    }

}
