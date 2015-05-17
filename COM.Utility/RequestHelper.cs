using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace COM.Utility
{
    /// <summary>     
    /// 1. 功能：缓存操作公共类
    /// 2. 作者：kaixuan
    /// 3. 创建日期：2014-10-24
    /// 4. 最后修改日期：2014-10-24
    /// </summary>  
    public class RequestHelper
    {
        /// <summary>
        /// 获取指定网页的HTML代码
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static string GetPageSource(string URL)
        {
            Uri uri = new Uri(URL);

            HttpWebRequest hwReq = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse hwRes = (HttpWebResponse)hwReq.GetResponse();

            hwReq.Method = "Get";
            hwReq.KeepAlive = false;
            //将该属性设置为 true 以发送带有 Keep-alive 值的 Connection HTTP 标头。
            //应用程序使用 KeepAlive 指示持久连接的首选项。
            //当 KeepAlive 属性为 true 时，应用程序与支持它们的服务器建立持久连接。
            //注意    使用 HTTP/1.1 时，Keep-Alive 默认情况下处于打开状态。
            //将 KeepAlive 设置为假可能导致将 Connection: Close 标头发送到服务器。

            using (StreamReader reader = new StreamReader(hwRes.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8")))
            {
                return reader.ReadToEnd();
            }
        }



        /// <summary>
        ///  //获取当前域名加HTTP
        /// </summary>
        public static string HttpDomain
        {
            get
            {
                return Http + Domain;
            }
        }

        /// <summary>
        /// 域名
        /// </summary>
        public static string Domain
        {
            get
            {
                if (((HttpContext.Current.Request.ServerVariables["SERVER_PORT"] == null) || (HttpContext.Current.Request.ServerVariables["SERVER_PORT"] == "")) || (HttpContext.Current.Request.ServerVariables["SERVER_PORT"] == "80"))
                {
                    return HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                }
                return (HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + ":" + HttpContext.Current.Request.ServerVariables["SERVER_PORT"]);
            }
        }
        /// <summary>
        /// http or https
        /// </summary>
        public static string Http
        {
            get
            {
                if (HttpContext.Current.Request.ServerVariables["HTTPS"].ToLower() == "on")
                {
                    return "https://";
                }
                return "http://";
            }
        }
        /// <summary>
        /// 当前 HTTP 请求的页面 URL 
        /// </summary>
        public static string Page
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["URL"];
            }
        }
        /// <summary>
        /// 物理路径
        /// </summary>
        public static string PhysicalPath
        {
            get
            {
                string str = HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"];
                if (!str.EndsWith(@"\"))
                {
                    str = str + @"\";
                }
                return str;
            }
        }
        /// <summary>
        /// 当前 HTTP 请求的完整 URL （包含参数信息）
        /// </summary>
        public static string Url
        {
            get
            {
                return (HttpContext.Current.Request.ServerVariables["URL"] + "?" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"]);
            }
        }
        /// <summary>
        /// ip地址
        /// </summary>
        public static string UserAddress
        {
            get
            {
                if ((HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null) || (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == ""))
                {
                    return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
        }
        /// <summary>
        /// 虚拟目录
        /// </summary>
        public static string Virtual
        {
            get
            {
                if ((HttpContext.Current.Request.ApplicationPath != null) && HttpContext.Current.Request.ApplicationPath.EndsWith("/"))
                {
                    return HttpContext.Current.Request.ApplicationPath;
                }
                return (HttpContext.Current.Request.ApplicationPath + "/");
            }
        }

        /// <summary>
        /// 获取GET或POST参数
        /// </summary>
        /// <param name="parName"></param>
        /// <returns></returns>
        public static string QueryString(string name)
        {
            return HttpContext.Current.Request[name];
        }

        /// <summary>
        /// 根据URL字符串获取参数
        /// </summary>
        /// <returns></returns>
        public static string QueryStringByUrlString(string url, string paramName)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            url = Regex.Replace(url, @".*\?", "");
            var paraArry = url.Split('&');
            var dic = paraArry.Select(c => new KeyValuePair<string, string>(c.Split('=').First().Trim(), c.Split('=').Last().Trim())).ToList();
            if (dic.Any(c => c.Key == paramName))
                return dic.Single(c => c.Key == paramName).Value;
            else return "";
        }
    }
}
