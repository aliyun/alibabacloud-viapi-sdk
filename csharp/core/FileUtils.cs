using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using AlibabaCloud.SDK.Viapiutils20200401.Models;

namespace AlibabaCloud.SDK.VIAPI.Utils
{
    public class FileUtils
    {
        AlibabaCloud.SDK.Viapiutils20200401.Client client;
        string endpoint = "viapiutils.cn-shanghai.aliyuncs.com";
        string accessKeyId;
        Regex fileNameRegex = new Regex("\\w+.(jpg|gif|png|jpeg|bmp|mov|mp4|avi)");

        static Dictionary<string, FileUtils> map = new Dictionary<string, FileUtils>();
        private static object syncObj = new object();

        public static FileUtils getInstance(string accessKeyId, string accessKeySecret)
        {
            lock (syncObj)
            {
                string mapKey = accessKeyId + accessKeySecret;
                if (map.ContainsKey(mapKey))
                {
                    return map[mapKey];
                }
                else
                {
                    var fileUtils = new FileUtils(accessKeyId, accessKeySecret);
                    return fileUtils;
                }
            }
        }

        private FileUtils(string accessKeyId, string accessKeySecret)
        {
            AlibabaCloud.RPCClient.Models.Config config = new AlibabaCloud.RPCClient.Models.Config
            {
                RegionId = "cn-shanghai",
                AccessKeyId = accessKeyId,
                AccessKeySecret = accessKeySecret,
                Endpoint = "viapiutils.cn-shanghai.aliyuncs.com"
            };

            client = new AlibabaCloud.SDK.Viapiutils20200401.Client(config);
            this.accessKeyId = accessKeyId;
        }

        public string Upload(string filePath)
        {
            Stream ins;
            string fileName = "";
            if (filePath != null && (filePath.StartsWith("http://") || filePath.StartsWith("https://")))
            {
                filePath = System.Web.HttpUtility.UrlDecode(filePath, Encoding.UTF8);
                if (fileNameRegex.IsMatch(filePath.ToLower()))
                {
                    fileName = fileNameRegex.Matches(filePath.ToLower())[0].Value;
                }

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(filePath);
                httpWebRequest.Method = "GET";
                using (HttpWebResponse res = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    ins = new MemoryStream();
                    using (var stream = res.GetResponseStream())
                    {

                        var buffer = new byte[1024];
                        while (true)
                        {
                            var length = stream.Read(buffer, 0, 1024);
                            if (length == 0)
                            {
                                break;
                            }

                            ins.Write(buffer, 0, length);
                        }
                    }
                }
            }
            else
            {
                fileName = System.IO.Path.GetFileName(filePath);
                ins = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }

            return Upload(fileName, ins);

        }

        public string Upload(string fileName, Stream stream)
        {
            try
            {
                GetOssStsTokenRequest getOssStsTokenRequest = new GetOssStsTokenRequest();
                var getOssStsTokenResponse = client.GetOssStsToken(getOssStsTokenRequest);
                string akKey = getOssStsTokenResponse.Data.AccessKeyId;
                string akSec = getOssStsTokenResponse.Data.AccessKeySecret;
                string token = getOssStsTokenResponse.Data.SecurityToken;

                string key = accessKeyId + "/" + Guid.NewGuid().ToString() + fileName;
                OSS.Models.Config ossConfig = new OSS.Models.Config();
                ossConfig.AccessKeyId = akKey;
                ossConfig.AccessKeySecret = akSec;
                ossConfig.SecurityToken = token;
                ossConfig.Endpoint = "oss-cn-shanghai.aliyuncs.com";
                ossConfig.Type = "sts";
                OSS.Client ossClient = new OSS.Client(ossConfig);
                OSS.Models.PutObjectRequest request = new OSS.Models.PutObjectRequest();
                request.BucketName = "viapi-customer-temp";
                request.Body = stream;
                request.ObjectName = key;
                var ossRes = ossClient.PutObject(request, new OSSUtil.Models.RuntimeOptions());
                return "http://viapi-customer-temp.oss-cn-shanghai.aliyuncs.com/" + key;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
    }
}
