using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Routing;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace COM.Extension
{
    /// <summary>     
    /// 1. 功能：string 常用操作
    /// 2. 作者：kaixuan
    /// 3. 创建日期：2014-10-24
    /// 4. 最后修改日期：2014-10-24
    /// </summary>  
    public static class ExtString
    {

        #region 获取脚本
        /// <summary>
        ///弹出 alert; 返回的是完整JS对象
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static HtmlString ToAlert(this string o)
        {
            string js = "alert('" + o + "')";
            return js.ToJavaScript();
        }
        /// <summary>
        ///弹出 alert;并且跳转
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static HtmlString ToAlertAndHref(this string message, string url)
        {
            string js = "alert(\"" + message + "\");window.location.href=\"" + url + "\"";
            return js.ToJavaScript();
        }
        /// <summary>
        /// 弹出提示并返回
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static HtmlString ToAlertBack(this string message)
        {
            return "alert('{0}');history.go(-1)".ToFormat(message).ToJavaScript();
        }
        /// <summary>
        /// 手机格式转换 13812341234 转成138-1234-1234
        /// </summary>
        /// <param name="pn">手机号</param>
        /// <returns></returns> 
        /// <summary>
        /// 调用js用 "alert(1)".ToJavaScript(); 返回的是完整JS对象
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static HtmlString ToJavaScript(this string o)
        {
            return new HtmlString("<script>" + o + "</script>");
        }
        #endregion
        #region 字符串格式化
        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static string ToFormat(this string o, params object[] ps)
        {
            return string.Format(o, ps);
        }
        #endregion
        #region 过滤SQL注入
        /// <summary>
        /// sql过滤关键字   
        /// </summary>
        /// <param name="objWord"></param>
        /// <returns></returns>
        public static string ToSqlFilter(this object objWord)
        {
            var str = objWord + "";
            str = str.Replace("'", "‘");
            str = str.Replace(";", "；");
            str = str.Replace(",", ",");
            str = str.Replace("?", "?");
            str = str.Replace("<", "＜");
            str = str.Replace(">", "＞");
            str = str.Replace("(", "(");
            str = str.Replace(")", ")");
            str = str.Replace("@", "＠");
            str = str.Replace("=", "＝");
            str = str.Replace("+", "＋");
            str = str.Replace("*", "＊");
            str = str.Replace("&", "＆");
            str = str.Replace("#", "＃");
            str = str.Replace("%", "％");
            str = str.Replace("$", "￥");
            return str;
        }
        #endregion
        #region 根据字符串分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static List<string> Split(this string str, string spiltStr)
        {
            var reval = str.Split(new string[] { spiltStr }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return reval;
        }
        #endregion
        #region  去除字符串中/t/r/n/s
        /// <summary>
        ///去除字符串中/t/r/n/s
        /// </summary>
        /// <param name="o"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToRemovetsrn(this string o)
        {
            if (o == null || o == "") return o;
            else
            {
                o = Regex.Replace(o, @"\t|\n|\s|\r", "");
            }
            return o;
        }
        /// <summary>
        ///去除字符串中/t/r/n/s
        /// </summary>
        /// <param name="o"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToRemovetsrn(this string o, string str)
        {
            o = o.Replace(" ", "");
            if (o == null || o == "") return str;
            else
            {
                o = Regex.Replace(o, @"\t|\n|\s|\r", "");
            }
            return o;
        }
        #endregion
        #region 替换html中的特殊字符
        ///<summary>
        ///替换html中的特殊字符
        ///</summary>
        public static string HtmlEncode(this string theString)
        {
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace(" ", "&nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("\'", "&#39;");
            theString = theString.Replace("\n", "<br/>");
            return theString;
        }
        #endregion
        #region 恢复html中的特殊字符
        ///<summary>
        ///恢复html中的特殊字符
        ///</summary>
        public static string HtmlDiscode(this string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "\'");
            theString = theString.Replace("<br/>", "\n");
            return theString;
        }
        #endregion
        #region 转成htmlstring
        public static HtmlString ToHtmlString(this string str)
        {
            return new HtmlString(str);
        }
        #endregion
        #region url编辑
        /// <summary>
        /// url编辑
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(this string str)
        {
            return HttpContext.Current.Server.UrlEncode(str);
        }
        /// <summary>
        /// url解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlDecode(this string str)
        {
            return HttpContext.Current.Server.HtmlDecode(str);
        }
        #endregion

    }
}
