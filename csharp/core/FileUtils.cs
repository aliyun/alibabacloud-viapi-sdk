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

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(fileName);
                httpWebRequest.Method = "GET";
                using (HttpWebResponse res = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    ins = res.GetResponseStream();
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
                Aliyun.OSS.OssClient ossClient = new Aliyun.OSS.OssClient("http://oss-cn-shanghai.aliyuncs.com", akKey, akSec, token);
                string key = accessKeyId + "/" + Guid.NewGuid().ToString() + fileName;
                ossClient.PutObject("viapi-customer-temp", key, stream);
                return "http://viapi-customer-temp.oss-cn-shanghai.aliyuncs.com/" + key;
            }
            finally
            {
                if(stream != null)
                {
                    stream.Close();
                }
            }
        }
    }
}
