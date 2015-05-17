using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.Utility
{
    /// <summary>
    /// 常用正则集合
    /// </summary>
    public class RegList
    {
        /// <summary>
        /// money类型
        /// </summary>
        public static string MONEY = @"^(-?\d+)(\.\d{0,2})?$";
        /// <summary>
        /// money类型可以为空
        /// </summary>
        public static string MONEY_NULL = @"^(-?\d+)(\.\d{0,2})?$|^.{0}$";
        /// <summary>
        /// money提示
        /// </summary>
        public static string MONEY_TIP = "格式为数字,可以带2位小数点！";

        /// <summary>
        /// 非空类型
        /// </summary>
        public static string REQUIRED=".+";
        /// <summary>
        /// 非空类型提示
        /// </summary>
        public static string REQUIRED_TIP = "必填";
    }
}
