using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.Utility
{
    /// <summary>     
    /// 1. 功能：生成随机值(数字、汉字、英文)
    /// 2. 作者：kaixuan
    /// 3. 创建日期：2014-10-31
    /// 4. 最后修改日期：2014-10-31
    /// </summary>  
    public class RandomHelper
    {
        #region 数字随机数
        /// <summary> 
        /// 数字随机数 
        /// </summary> 
        /// <param name="n">生成长度</param> 
        /// <returns></returns> 
        public static string RandNum(int n)
        {
            char[] arrChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, 9)].ToString());
            }
            return num.ToString();
        }
        #endregion
        #region 数字和字母随机数
        /// <summary> 
        /// 数字和字母随机数 
        /// </summary> 
        /// <param name="n">生成长度</param> 
        /// <returns></returns> 
        public static string RandCode(int n)
        {
            char[] arrChar = new char[]{ 
'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x', 
'0','1','2','3','4','5','6','7','8','9', 
'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z' 
};
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
        #endregion
        #region 字母随机数
        /// <summary> 
        /// 字母随机数 
        /// </summary> 
        /// <param name="n">生成长度</param> 
        /// <returns></returns> 
        public static string RandLetter(int n)
        {
            char[] arrChar = new char[]{ 
'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x', 
'_', 
'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z' 
};
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
        #endregion
        #region 日期随机函数
        /// <summary> 
        /// 日期随机函数 
        /// </summary> 
        /// <param name="ra">长度</param> 
        /// <returns></returns> 
        public static string DateRndName(Random ra)
        {
            DateTime d = DateTime.Now;
            string s = null, y, m, dd, h, mm, ss;
            y = d.Year.ToString();
            m = d.Month.ToString();
            if (m.Length < 2) m = "0" + m;
            dd = d.Day.ToString();
            if (dd.Length < 2) dd = "0" + dd;
            h = d.Hour.ToString();
            if (h.Length < 2) h = "0" + h;
            mm = d.Minute.ToString();
            if (mm.Length < 2) mm = "0" + mm;
            ss = d.Second.ToString();
            if (ss.Length < 2) ss = "0" + ss;
            s += y + m + dd + h + mm + ss;
            s += ra.Next(100, 999).ToString();
            return s;
        }
        #endregion
        #region 生成GUID
        /// <summary> 
        /// 生成GUID 
        /// </summary> 
        /// <returns></returns> 
        public static string GetGuid()
        {
            System.Guid g = System.Guid.NewGuid();
            return g.ToString();
        }
        #endregion
        #region 生成汉字字符
        /// <summary>
        /// 生成汉字字符
        /// </summary>
        /// <returns></returns>
        public string CreateZhChar(int length)
        {
            //若提供了汉字集，查询汉字集选取汉字
            //if (ChineseChars.Length > 0)
            //{
            //    return ChineseChars[rnd.Next(0, ChineseChars.Length)];
            //}
            //若没有提供汉字集，则根据《GB2312简体中文编码表》编码规则构造汉字
            //else
            //{    
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < length; i++)
            {
                Random rnd = new Random((int)DateTime.Now.Ticks + i);
                byte[] bytes = new byte[2];

                //第一个字节值在0xb0, 0xf7之间
                bytes[0] = (byte)rnd.Next(0xb0, 0xf8);
                //第二个字节值在0xa1, 0xfe之间
                bytes[1] = (byte)rnd.Next(0xa1, 0xff);
                //根据汉字编码的字节数组解码出中文汉字
                string str1 = System.Text.Encoding.GetEncoding("gb2312").GetString(bytes);

                //}
                sb.Append(str1);
            }
            return sb.ToString();
        }

        #endregion
    }
}
