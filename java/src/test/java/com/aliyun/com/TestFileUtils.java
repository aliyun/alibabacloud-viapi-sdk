package com.aliyun.com;

import java.io.IOException;

import com.aliyun.com.viapi.FileUtils;
import com.aliyuncs.exceptions.ClientException;

public class TestFileUtils {

    public void testLocalFile() throws ClientException, IOException {
        String accessKey = "xxx";
        String accessKeySecret = "xxx";
        //String file = /home/admin/file/1.jpg
        String file = "http://xxx.jpg";
        FileUtils fileUtils = FileUtils.getInstance(accessKey,accessKeySecret);
        String ossurl = fileUtils.upload(file);
    }

}
