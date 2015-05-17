using System;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
namespace COM.Utility
{

    /// <summary>     
    /// 1. 功能：get post cookies进行安全验证
    /// 2. 作者：kaixuan
    /// 3. 创建日期：2014-10-24
    /// 4. 最后修改日期：2014-10-24
    /// </summary>  
    public class SafeValidates
    {

        #region  helper
        private const string StrRegex = @"<[^>]+?style=[\w]+?:expression\(|\b(alert|confirm|prompt)\b|^\+/v(8|9)|<[^>]*?=[^>]*?&#[^>]*?>|\b(and|or)\b.{1,6}?(=|>|<|\bin\b|\blike\b)|/\*.+?\*/|<\s*script\b|<\s*img\b|\bEXEC\b|UNION.+?SELECT|UPDATE.+?SET|INSERT\s+INTO.+?VALUES|(SELECT|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\s+(TABLE|DATABASE)";

        private static bool PostData()
        {
            bool result = false;

            try
            {
                for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
                {
                    result = CheckData(HttpContext.Current.Request.Form[i].ToString());
                    if (result)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {

                return false;
            }
            return result;
        }


        private static bool GetData()
        {
            bool result = false;

            for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
            {
                result = CheckData(HttpContext.Current.Request.QueryString[i].ToString());
                if (result)
                {
                    break;
                }
            }
            return result;
        }


        private static bool CookieData()
        {
            bool result = false;
            for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
            {
                result = CheckData((HttpContext.Current.Request.Cookies[i].Value + "").ToLower());
                if (result)
                {
                    break;
                }
            }
            return result;

        }
        private static bool referer()
        {
            bool result = false;
            return result = CheckData(HttpContext.Current.Request.UrlReferrer.ToString());
        }

        private static bool CheckData(string inputData)
        {
            if (Regex.IsMatch(inputData, StrRegex) || ((inputData + "").Contains("'")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 请求安全验证
        /// Safe.CheckRequest(r =>
        ///  {
        ///    //不安全
        ///    //跳转到
        /// 
        ///  })
        ///  安全//
        /// </summary>
        public static bool CheckRequest(Action<int> a)
        {
            var request = HttpContext.Current.Request;
            var response = HttpContext.Current.Response;

            if (request.Cookies != null)
            {
                if (SafeValidates.CookieData())
                {
                    a(0);
                    return false;
                }
            }

            if (request.UrlReferrer != null)
            {
                if (SafeValidates.referer())
                {
                    a(0);
                    return false;
                }
            }

            if (request.RequestType.ToUpper() == "POST")
            {
                if (SafeValidates.PostData())
                {

                    a(0);
                    return false;
                }
            }
            if (request.RequestType.ToUpper() == "GET")
            {
                if (SafeValidates.GetData())
                {
                    a(0);
                    return false;
                }
            }
            return true;
        }

    }
}
