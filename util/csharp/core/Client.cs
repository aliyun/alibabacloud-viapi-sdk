/**
 * For viapi
 */
// This file is auto-generated, don't edit it. Thanks.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Tea;
using Tea.Utils;


namespace AlibabaCloud.ViapiTool
{
    public class Client 
    {
        private static readonly HttpClient httpClient = new HttpClient();

        /**
         * Judge whether origin starts with sub, if yes, return true, or return false
         * @param origin the original string
         * @param sub the prefix string
         * @return the judge result
         */
        public static bool? StartsWith(string origin, string sub)
        {
            if(string.IsNullOrEmpty(origin))
            {
                return false;
            }
            return origin.StartsWith(sub);
        }

        /**
         * If whether origin contains regrex, return string, or return ''
         * @param origin the original string
         * @param regrex the regular expression
         * @return the matched string
         */
        public static string Match(string origin, string regrex)
        {
            var match = Regex.Match(origin, regrex);
            if (match.Success)
            {
                return match.Value;
            }
            return string.Empty;
        }

        /**
         * Decode origin with decodeType
         * @param origin the original string
         * @param decodeType decode type
         * @return the decoded string
         */
        public static string Decode(string origin, string decodeType)
        {
            return HttpUtility.UrlDecode(origin, System.Text.Encoding.GetEncoding(decodeType));
        }

        /**
         * Get file name from url
         * @param urlStr the url
         * @return the file name
         */
        public static string GetNameFromUrl(string urlStr)
        {
            Uri uri = new Uri(urlStr);
            return uri.AbsolutePath;
        }

        /**
         * Get file name from  path
         * @param pathStr the path
         * @return the file name
         */
        public static string GetNameFromPath(string pathStr)
        {
            return Path.GetFileName(pathStr);
        }

        /**
         * Intercepts the last string according to sub
         * @param path the original string
         * @param sub the cutting string
         * @return the last string
         */
        public static string SubStringAfterLast(string path, string sub)
        {
            if (string.IsNullOrEmpty(sub))
            {
                return path;
            }
            int index = path.LastIndexOf(sub);
            if (index < 0)
            {
                //No such substring
                return string.Empty;
            }
            return path.Substring(index + sub.Length);
        }

        /**
         * Get stream from net
         * @param url the file url
         * @return the stream
         */
        public static Stream GetStreamFromNet(string url)
        {
            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            return response.Content.ReadAsStreamAsync().Result;
        }

        /**
         * Get stream from net
         * @param url the file url
         * @return the stream
         */
        public static async Task<Stream> GetStreamFromNetAsync(string url)
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            return await response.Content.ReadAsStreamAsync();
        }

        /**
         * Get stream from path
         * @param filepath the file path
         * @return the stream
         */
        public static Stream GetStreamFromPath(string filepath)
        {
            FileStream fileStream = File.OpenRead(filepath);
            return fileStream;
        }

        /**
         * Get stream from path
         * @param filepath the file path
         * @return the stream
         */
        public static async Task<Stream> GetStreamFromPathAsync(string filepath)
        {
            return await Task.Factory.StartNew(() =>
            {
                FileStream fileStream = File.OpenRead(filepath);
                return fileStream;
            });
        }

        /**
         * Close the body
         * @param body the stream that needs to be closed
         */
        public static void Close(Stream body)
        {
            body.Close();
        }

    }
}
