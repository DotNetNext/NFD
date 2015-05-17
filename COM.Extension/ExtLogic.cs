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
    /// 1. 功能：常用判段函数
    /// 2. 作者：kaixuan
    /// 3. 创建日期：2014-10-24
    /// 4. 最后修改日期：2014-10-24
    /// </summary>   
    public static class ExtLogic
    {

        /// <summary>
        /// o==true 返回successVal否则返回errorVal
        /// </summary>
        /// <param name="o"></param>
        /// <param name="successVal"></param>
        /// <param name="errorVal"></param>
        /// <returns></returns>
        public static string IIF(this object o, string successVal, string errorVal)
        {
            return Convert.ToBoolean(o) ? successVal : errorVal;
        }

        /// <summary>
        /// o==true 返回successVal否则返回""
        /// </summary>
        /// <param name="o"></param>
        /// <param name="successVal"></param>
        /// <param name="errorVal"></param>
        /// <returns></returns>
        public static string IIF(this object o, string successVal)
        {
            return Convert.ToBoolean(o) ? successVal : "";
        }

        /// <summary>
        /// o==true 执行success 否则执行 error
        /// </summary>
        /// <param name="o"></param>
        /// <param name="success"></param>
        /// <param name="error"></param>
        public static void IIF(this object o, Action<dynamic> success, Action<dynamic> error)
        {
            if (Convert.ToBoolean(o))
            {
                success(o);
            }
            else
            {
                error(error);
            }
        }

        /// <summary>
        /// 是否包含于指定数组 
        /// 例如:
        /// string a="abc";
        /// a.IsContains("bbb","abc") 则返回true 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="os">包含数组</param>
        /// <returns></returns>
        public static bool IsContains(this object o, params object[] os)
        {
            return os.Contains(o);
        }



        /// <summary>
        /// 判段值的范围
        /// </summary>
        /// <param name="val"></param>
        /// <param name="begin">大于等于begin</param>
        /// <param name="end">小于等于end</param>
        /// <returns></returns>
        public static bool IsArea(this int val, int begin, int end)
        {
            return val >= begin && val <= end;
        }
        /// <summary>
        /// 判段值的范围
        /// </summary>
        /// <param name="val"></param>
        /// <param name="begin">大于等于begin</param>
        /// <param name="end">小于等于end</param>
        /// <returns></returns>
        public static bool IsArea(this DateTime val, DateTime begin, DateTime end)
        {
            return val >= begin && val <= end;
        }

        /// <summary>
        /// 判段值的范围
        /// </summary>
        /// <param name="val"></param>
        /// <param name="begin">大于等于begin</param>
        /// <param name="end">小于等于end</param>
        /// <returns></returns>
        public static bool IsArea(string val, string pattern)
        {
            Regex reg = new Regex(pattern);
            return reg.IsMatch(val);
        }

        /// <summary>
        /// 是否是null或""
        /// </summary>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object o)
        {
            if (o == null || o == DBNull.Value) return true;
            return o.ToString() == "";
        }
        /// <summary>
        /// 是否是null或""
        /// </summary>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this Guid? o)
        {
            if (o == null) return true;
            return o == Guid.Empty;
        }
        /// <summary>
        /// 是否是null或""
        /// </summary>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this Guid o)
        {
            if (o == null) return true;
            return o == Guid.Empty;
        }

        /// <summary>
        /// 是否有值(与IsNullOrEmpty相反)
        /// </summary>
        /// <returns></returns>
        public static bool IsHasValue(this object o)
        {
            if (o == null) return false;
            return o.ToString() != "";
        }

        /// <summary>
        /// 是否为零
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsZero(this object o)
        {
            return (o == null || o.ToString() == "0");
        }

        /// <summary>
        /// 是否是INT
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsInt(this object o)
        {
            if (o == null) return false;
            return Regex.IsMatch(o.ToString(), @"^\d+$");
        }
        /// <summary>
        /// 是否不是INT
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsNoInt(this object o)
        {
            if (o == null) return true;
            return !Regex.IsMatch(o.ToString(), @"^\d+$");
        }

        /// <summary>
        /// 是否是邮箱
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsEamil(this object o)
        {
            if (o == null) return false;
            return Regex.IsMatch(o.ToString(), @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");
        }

        /// <summary>
        /// 是否是手机
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsMobile(this object o)
        {
            if (o == null) return false;
            return Regex.IsMatch(o.ToString(), @"^\d{11}$");
        }

        /*一些常用的正则表达式
             * 
             * 
             * 
                ^\d+$　　//匹配非负整数（正整数 + 0） 
                ^[0-9]*[1-9][0-9]*$　　//匹配正整数 
                ^((-\d+)|(0+))$　　//匹配非正整数（负整数 + 0） 
                ^-[0-9]*[1-9][0-9]*$　　//匹配负整数 
                ^-?\d+$　　　　//匹配整数 
                ^\d+(\.\d+)?$　　//匹配非负浮点数（正浮点数 + 0） 
                ^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$　　//匹配正浮点数 
                ^((-\d+(\.\d+)?)|(0+(\.0+)?))$　　//匹配非正浮点数（负浮点数 + 0） 
                ^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$　　//匹配负浮点数 
                ^(-?\d+)(\.\d+)?$　　//匹配浮点数 
                ^[A-Za-z]+$　　//匹配由26个英文字母组成的字符串 
                ^[A-Z]+$　　//匹配由26个英文字母的大写组成的字符串 
                ^[a-z]+$　　//匹配由26个英文字母的小写组成的字符串 
                ^[A-Za-z0-9]+$　　//匹配由数字和26个英文字母组成的字符串 
                ^\w+$　　//匹配由数字、26个英文字母或者下划线组成的字符串 
                ^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$　　　　//匹配email地址 
                ^[a-zA-z]+://匹配(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$　　//匹配url 
                ^\d{11}$ //匹配手机
                匹配中文字符的正则表达式： [\u4e00-\u9fa5] 
                匹配双字节字符(包括汉字在内)：[^\x00-\xff] 
                匹配空行的正则表达式：\n[\s| ]*\r 
                匹配HTML标记的正则表达式：/<(.*)>.*<\/>|<(.*) \/>/ 
                匹配首尾空格的正则表达式：(^\s*)|(\s*$) 
                匹配Email地址的正则表达式：\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)* 
                匹配网址URL的正则表达式：^[a-zA-z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$ 
                匹配帐号是否合法(字母开头，允许5-16字节，允许字母数字下划线)：^[a-zA-Z][a-zA-Z0-9_]{4,15}$ 
                匹配国内电话号码：(\d{3}-|\d{4}-)?(\d{8}|\d{7})? 
                匹配腾讯QQ号：^[1-9]*[1-9][0-9]*$ 
             * */
    }
}
