using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;

namespace COM.Utility
{
     
    /// <summary>
    /// 1. 功能：用于字符串处理的公共操作类
    /// 2. 作者：kaixuan
    /// 3. 创建日期：2008-2-16
    /// 4. 最后修改日期：2008-12-6
    /// </summary>
    [Serializable]
    public sealed class StringHelper
    {
        #region 字段定义
        /// <summary>
        /// 要生成的字符串
        /// </summary>
        private StringBuilder _str;
        /// <summary>
        /// 缩进级别
        /// </summary>
        private int _indentLevel = 0;
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化对象
        /// </summary>
        public StringHelper()
        {
            //初始化字符串
            _str = new StringBuilder();
        }
        #endregion

        #region 添加字符串
        /// <summary>
        /// 添加字符串
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="parameters">参数</param>
        public void Append(string text, params object[] parameters)
        {
            //带参数的
            _str.AppendFormat(text, parameters);

        }
        /// <summary>
        /// 添加字符串不带参数
        /// </summary>
        /// <param name="text">文本</param>
        public void Append(string text)
        {
            _str.Append(text);
        }

        #endregion

        #region 清空字符串
        /// <summary>
        /// 清空字符串
        /// </summary>
        public void Clear()
        {
            Clear(_str);
        }
        #endregion

        #region 字符串长度
        /// <summary>
        /// 字符串长度
        /// </summary>
        public int Length
        {
            get
            {
                return _str.Length;
            }
        }
        #endregion

        #region 增加缩进
        /// <summary>
        /// 增加缩进
        /// </summary>
        public void IncreaseIndent()
        {
            _indentLevel++;
        }

        /// <summary>
        /// 增加缩进
        /// </summary>
        /// <param name="step">增加量</param>
        public void IncreaseIndent(int step)
        {
            _indentLevel += step;
        }
        #endregion

        #region 减少缩进
        /// <summary>
        /// 减少缩进
        /// </summary>
        public void DecreaseIndent()
        {
            _indentLevel--;
        }
        #endregion

        #region 移除末尾指定字符串
        /// <summary>
        /// 移除字符串末尾指定字符串
        /// </summary>
        /// <param name="removeString">要移除的字符串</param>
        public void RemoveEnd(string removeString)
        {
            //创建临时字符串
            string tempString = _str.ToString().TrimEnd(removeString.ToCharArray());

            if (tempString != _str.ToString())
            {
                StringBuilder temp = new StringBuilder();
                temp.Append(tempString);

                //为字段赋值
                _str = temp;
            }
        }
        #endregion

        #region 静态方法

        #region 将字符转换为ASCII
        public static int Asc(string character)
        {
            if (character.Length == 1)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int intAsciiCode = (int)asciiEncoding.GetBytes(character)[0];
                return (intAsciiCode);
            }
            else
            {
                throw new Exception("Character is not valid.");
            }

        }
        #endregion

        #region 将ASCII转换为字符
        public static string UnAsc(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                throw new Exception("ASCII Code is not valid.");
            }
        }



        #endregion

        #region 对脚本中的消息进行过滤处理
        /// <summary>
        /// 对脚本中的消息进行过滤处理
        /// </summary>
        /// <param name="msg">脚本中的消息字符串,但不包括脚本函数.
        /// 范例：alert('ab\n\rc'),只传入ab\n\rc,不要把alert('')传进来。</param>
        public static string GetValidScriptMsg(string msg)
        {
            StringBuilder validMsg = new StringBuilder(msg);
            validMsg.Replace("\\", @"\\");
            validMsg.Replace("\n", "\\n");
            validMsg.Replace("\t", "\\t");
            validMsg.Replace("\r", "\\r");
            validMsg.Replace("'", "\\'");

            //返回有效的字符串
            return validMsg.ToString();
        }
        #endregion

        #region 去除字符串最后的逗号
        /// <summary>
        /// 去除字符串最后的逗号,返回新的字符串
        /// </summary>
        /// <param name="text">原始字符串</param>
        public static string RemoveLastComma(ref string text)
        {
            text = text.TrimEnd(',');
            return text;
        }

        /// <summary>
        /// 去除字符串最后的逗号,返回新的字符串
        /// </summary>
        /// <param name="text">原始字符串</param>
        public static string RemoveLastComma(StringBuilder text)
        {
            return text.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 去除字符串最后的逗号,返回新的字符串
        /// </summary>
        /// <param name="text">原始字符串</param>
        public static string RemoveLastComma(StringHelper text)
        {
            return text.ToString().TrimEnd(',');
        }
        #endregion

        #region 清空字符串
        /// <summary>
        /// 清空字符串
        /// </summary>
        /// <param name="text">原始字符串</param>
        public static void Clear(StringBuilder text)
        {
            text.Remove(0, text.Length);
        }
        #endregion

        #region 获取字符串的最后一个字符
        /// <summary>
        /// 获取字符串的最后一个字符
        /// </summary>
        /// <param name="text">原始字符串</param>        
        public static string GetLastChar(string text)
        {
            //如果字符串为空，则返回
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            return text.Substring(text.Length - 1, 1);
        }
        #endregion

        #region 用逗号拼接数组

        #region 重载方法1
        /// <summary>
        /// 获取用逗号拼接数组元素的字符串。返回字符串的数组元素默认不带引号，如果需要引号请使用重载方法。
        /// 范例: 数组为:string[] a = new string[] { "1","2","3" };返回值:1,2,3
        /// </summary>
        /// <param name="stringArray">原始字符串数组</param>
        public static string GetCommaString(params string[] stringArray)
        {
            return GetCommaString(false, stringArray);
        }
        #endregion

        #region 重载方法2
        /// <summary>
        /// 获取用逗号拼接数组元素的字符串。
        /// 范例: 数组为:string[] a = new string[] { "1","2","3" };返回值:1,2,3
        /// </summary>
        /// <param name="isAddQuotationMarks">是否添加单引号,如果传入true，则返回值为'1','2','3'</param>
        /// <param name="stringArray">原始字符串数组</param>
        public static string GetCommaString(bool isAddQuotationMarks, params string[] stringArray)
        {
            //临时字符串
            StringBuilder list = new StringBuilder();

            //循环字符串数组，添加到临时字符串中
            foreach (string text in stringArray)
            {
                if (isAddQuotationMarks)
                {
                    list.AppendFormat("'{0}',", text);
                }
                else
                {
                    list.AppendFormat("{0},", text);
                }
            }

            //返回字符串
            return RemoveLastComma(list);
        }
        #endregion

        #region 重载方法3
        /// <summary>
        /// 获取用逗号拼接数组元素的字符串。返回字符串的数组元素默认不带引号，如果需要引号请使用重载方法。
        /// 范例: 数组为:int[] a = new int[] { 1,2,3 };返回值:1,2,3
        /// </summary>
        /// <param name="intArray">原始整型数组</param>
        public static string GetCommaString(params int[] intArray)
        {
            return GetCommaString(false, intArray);
        }
        #endregion

        #region 重载方法4
        /// <summary>
        /// 获取用逗号拼接数组元素的字符串。
        /// 范例: 数组为:int[] a = new int[] { 1,2,3 };返回值:1,2,3
        /// </summary>
        /// <param name="isAddQuotationMarks">是否添加单引号,如果传入true，则返回值为'1','2','3'</param>
        /// <param name="intArray">原始整型数组</param>
        public static string GetCommaString(bool isAddQuotationMarks, params int[] intArray)
        {
            //临时字符串
            StringBuilder list = new StringBuilder();

            //循环字符串数组，添加到临时字符串中
            foreach (int text in intArray)
            {
                if (isAddQuotationMarks)
                {
                    list.AppendFormat("'{0}',", text);
                }
                else
                {
                    list.AppendFormat("{0},", text);
                }
            }

            //返回字符串
            return RemoveLastComma(list);
        }
        #endregion

        #region 重载方法5
        /// <summary>
        /// 获取用逗号拼接元素的字符串。返回字符串的元素默认不带引号，如果需要引号请使用重载方法。
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="columnName">列名</param>
        /// <param name="isAddQuotationMarks">是否添加单引号,如果传入true，则返回值为'1','2','3'</param>
        public static string GetCommaString(DataTable table, string columnName)
        {
            return GetCommaString(table, columnName, false);
        }
        #endregion

        #region 重载方法6
        /// <summary>
        /// 获取用逗号拼接元素的字符串。返回字符串的元素默认不带引号，如果需要引号请使用重载方法。
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="columnName">列名</param>
        /// <param name="isAddQuotationMarks">是否添加单引号,如果传入true，则返回值为'1','2','3'</param>
        public static string GetCommaString(DataTable table, string columnName, bool isAddQuotationMarks)
        {
            //临时字符串
            StringBuilder list = new StringBuilder();

            //循环数据集，添加到临时字符串中
            foreach (DataRow row in table.Rows)
            {
                if (isAddQuotationMarks)
                {
                    list.AppendFormat("'{0}',", row[columnName].ToString());
                }
                else
                {
                    list.AppendFormat("{0},", row[columnName].ToString());
                }
            }

            //返回字符串
            return RemoveLastComma(list);
        }
        #endregion

        #endregion

        #region 获取数字格式的当前时间
        /// <summary>
        /// 获取数字格式的当前时间
        /// </summary>
        public static string GetNumberByNowDate()
        {
            //获取当前时间的字符串表示
            string numberTime = DateTime.Now.ToString("g");

            //转换为纯数字
            numberTime = numberTime.Replace("-", "");
            numberTime = numberTime.Replace(":", "");
            numberTime = numberTime.Replace(" ", "");

            //返回数字格式的当前时间
            return numberTime;
        }
        #endregion

        #region 将字符串的首字母大写
        /// <summary>
        /// 将字符串的首字母大写
        /// </summary>
        /// <param name="text">原始字符串</param>
        public static void FirstCharUpper(ref string text)
        {
            //获取首字母
            string temp = text.Substring(0, 1).ToUpper();

            //将字符串的首字母大写
            text = temp + text.Substring(1, text.Length - 1);
        }
        #endregion

        #region 获取百分比字符串
        /// <summary>
        /// 获取百分比字符串,返回值范例： 100%
        /// </summary>
        /// <param name="percent">0-100之间的整数</param>
        public static string GetPercentage(int percent)
        {
            return percent + "%";
        }
        #endregion

        #region 创建正则表达式的模式字符串
        /// <summary>
        /// 创建正则表达式的模式字符串,添加了开头和结尾的模式字符。
        /// 比如传入字符串"abc",返回"^abc$"
        /// </summary>
        /// <param name="text">原始字符串</param>
        public static string GetPattern(string text)
        {
            return "^" + text + "$";
        }
        #endregion

        #region 分解URL
        /// <summary>
        /// 分解URL,获取url路径中?的前部分( 去掉了查询字符串的部分 ) 与 查询字符串部分
        /// </summary>
        /// <param name="url">原始URL</param>
        /// <param name="hostUrl">去掉了查询字符串的URL</param>
        /// <param name="queryString">查询字符串</param>
        public static void DecomposeUrl(string url, out string hostUrl, out string queryString)
        {
            //如果重写的URL有查询字符串，则分解
            if (url.IndexOf("?") != -1)
            {
                //获取url路径中?的前部分
                hostUrl = url.Substring(0, url.IndexOf('?'));
                //获取查询字符串
                queryString = url.Substring(url.IndexOf('?') + 1);
            }
            else
            {
                //url路径中?的前部分
                hostUrl = url;
                //查询字符串
                queryString = string.Empty;
            }
        }
        #endregion

        #region 分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (strContent.IndexOf(strSplit) < 0)
            {
                string[] tmp = { strContent };
                return tmp;
            }
            return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int p_3)
        {
            string[] result = new string[p_3];

            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < p_3; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }
        #endregion

        #region 判断指定字符串是否属于指定字符串数组中的一个元素
        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                    {
                        return i;
                    }
                }
                else
                {
                    if (strSearch == stringArray[i])
                    {
                        return i;
                    }
                }

            }
            return -1;
        }


        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
        public static int GetInArrayID(string strSearch, string[] stringArray)
        {
            return GetInArrayID(strSearch, stringArray, true);
        }
        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">字符串数组</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string[] stringarray)
        {
            return InArray(str, stringarray, false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray)
        {
            return InArray(str, SplitString(stringarray, ","), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit)
        {
            return InArray(str, SplitString(stringarray, strsplit), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit, bool caseInsensetive)
        {
            return InArray(str, SplitString(stringarray, strsplit), caseInsensetive);
        }
        #endregion

        #region 从字符串的指定位置截取指定长度的子字符串
        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }


                if (startIndex > str.Length)
                {
                    return "";
                }


            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }


        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }
        /// <summary>
        /// 截取字符。不区分中英文
        /// </summary>
        /// <param name="p_SrcString"></param>
        /// <param name="p_Length"></param>
        /// <param name="bdot"></param>
        /// <returns></returns>
        public static string GetSubString(string p_SrcString, int p_Length, bool bdot)
        {
            string str = "";

            if (p_Length >= p_SrcString.Length)
                return p_SrcString;

            int nRealLen = p_Length * 2;
            if (bdot)
                nRealLen = nRealLen - 3;

            Encoding encoding = Encoding.GetEncoding("gb2312");
            for (int i = p_SrcString.Length; i >= 0; i--)
            {
                str = p_SrcString.Substring(0, i);
                if (encoding.GetBytes(str).Length > nRealLen)
                    continue;
                else
                    break;
            }

            if (bdot)
                str += "...";

            return str;
        }

        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }



                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {

                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                    {
                        nRealLength = p_Length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }

        /// <summary>
        /// 按一定字节长度截取字符串
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="length">要截取的字节长度</param>
        /// <returns>返回处理后的字符串，超出长度部分被""替换</returns>
        public static string GetStringByLength(string str, int length, string p_TailString)
        {
            byte[] bwrite = Encoding.GetEncoding("GB2312").GetBytes(str.ToCharArray());
            if (bwrite.Length >= length)
            {
                string NewStr = Encoding.Default.GetString(bwrite, 0, length - 4);
                if (NewStr[NewStr.Length - 1] == '?')
                {
                    NewStr = Encoding.Default.GetString(bwrite, 0, length - 3);
                }
                return NewStr + p_TailString;
            }
            else
                return str;
        }

        #endregion

        #region 返回字符串真实长度
        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns></returns>
        public static int GetStringLength(string Text)
        {
            int len = 0;

            for (int i = 0; i < Text.Length; i++)
            {
                byte[] byte_len = Encoding.Default.GetBytes(Text.Substring(i, 1));
                if (byte_len.Length > 1)
                    len += 2;  //如果长度大于1，是中文，占两个字节，+2
                else
                    len += 1;  //如果长度等于1，是英文，占一个字节，+1
            }

            return len;
        }
        #endregion

        #region 去除数组内重复元素
        /**/
        /// <summary>
        /// 去除数组内重复元素
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns> 
        public static ArrayList FilterRepeatArrayItem(ArrayList arr)
        {
            //建立新数组
            ArrayList newArr = new ArrayList();
            //载入第一个原数组元素
            if (arr.Count > 0)
            {
                newArr.Add(arr[0]);
            }
            //循环比较
            for (int i = 0; i < arr.Count; i++)
            {
                if (!newArr.Contains(arr[i]))
                {
                    newArr.Add(arr[i]);
                }
            }
            return newArr;
        }
        #endregion

        #region 字符超长处理
        public static string OverHide(string str, int maxLength, string appendStr)
        {
            bool isNotNull = str != null && str != string.Empty;
            if (isNotNull && GetStringLength(str) > maxLength)
            {
                if (str == null)
                {
                    return string.Empty;
                }
                int len = maxLength;
                int j = 0, k = 0;
                Encoding encoding = Encoding.GetEncoding("gb2312");
                for (int i = 0; i < str.Length; i++)
                {
                    byte[] bytes = encoding.GetBytes(str.Substring(i, 1));
                    if (bytes.Length == 2)//不是英文
                    {
                        j += 2;
                    }
                    else
                    {
                        j++;
                    }
                    if (j <= len)
                    {
                        k += 1;
                    }
                    if (j >= len)
                    {
                        return str.Substring(0, k) + appendStr;
                    }
                }
                return str;
            }
            return str;
        }
        #endregion

        #region 特殊字符的过虑
        /// <summary>
        /// 特殊字符的过虑
        /// </summary>
        /// <returns></returns>
        public static string BadChar(string strChar)
        {
            if (string.IsNullOrEmpty(strChar))
            {
                return null;
            }
            else
            {
                strChar = strChar.Replace("'", "");
                strChar = strChar.Replace("*", "");
                strChar = strChar.Replace("or", "");
                strChar = strChar.Replace("and", "");
                strChar = strChar.Replace(";", "；");
                strChar = strChar.Replace("&nbsp;", "");
                strChar = strChar.Replace("&quot;", "");
                strChar = strChar.Replace("%20", "");
                strChar = strChar.Replace("aspx", "");
                strChar = strChar.Replace("asp", "");
                strChar = strChar.Replace("exec", "");
                strChar = strChar.Replace("insert", "");
                strChar = strChar.Replace("select", "");
                strChar = strChar.Replace("update", "");
                strChar = strChar.Replace("truncate", "");
                strChar = strChar.Replace("net user", "");
                strChar = strChar.Replace("xp_cmdshell", "");
                strChar = strChar.Replace("net localgroup adminstrator", "");
                strChar = strChar.Replace("script", "");
                strChar = strChar.Replace("iframe", "");
                strChar = strChar.Replace("&#", "");
                strChar = strChar.Replace("char", "");
                strChar = strChar.Replace("declare", "");
                strChar = strChar.Replace("<script>", "");
                strChar = strChar.Replace("</script>", "");
                strChar = strChar.Replace("alert", "");
                return strChar;
            }

        }
        #endregion

        #region 过滤HTML代码
        public static string ReplaceHtml(string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" no[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"\<img[^\>]+\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex7 = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex8 = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex9 = new System.Text.RegularExpressions.Regex(@"<[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记 
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性 
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件 
            html = regex4.Replace(html, ""); //过滤iframe 
            html = regex5.Replace(html, ""); //过滤frameset 
            html = regex6.Replace(html, ""); //过滤frameset 
            html = regex7.Replace(html, ""); //过滤frameset 
            html = regex8.Replace(html, ""); //过滤frameset 
            html = regex9.Replace(html, "");
            html = html.Replace("&nbsp;", " ");
            html = html.Replace("&#40;", "(");
            html = html.Replace("&#41;", ")");
            html = html.Replace("\n\r", "");
            html = html.Replace("\r\n", "");
            html = html.Replace("\n", "");
            html = html.Replace("\r", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");

            html = html.Replace(" ", "");
            return html;
        }
        #endregion

        #region 替换,恢复html中的特殊字符
        /// <summary>
        /// 替换，恢复html中的特殊字符
        /// </summary>
        /// <param name="theString">需要进行替换的文本。</param>
        /// <returns>替换完的文本。</returns>
        public static string HtmlEncode(string theString)
        {
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace(" ", "&nbsp;");
            theString = theString.Replace(" ", "&nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("\'", "'");
            theString = theString.Replace("\n", "<br/> ");
            return theString;
        }

        /// <summary>
        /// 恢复html中的特殊字符
        /// </summary>
        /// <param name="theString">需要恢复的文本。</param>
        /// <returns>恢复好的文本。</returns>
        public static string HtmlDiscode(string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("'", "\'");
            theString = theString.Replace("<br/> ", "\n");
            return theString;
        }
        #endregion

        #region 替换字符串中是否有非法的字符
        /// <summary>
        /// 过滤非法字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceBadChar(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            string strBadChar, tempChar;
            string[] arrBadChar;
            strBadChar = "@@,+,',--,%,^,&,?,(,),<,>,[,],{,},/,\\,;,:,\",\"\"";
            arrBadChar = StringHelper.SplitString(strBadChar, ",");
            tempChar = str;
            for (int i = 0; i < arrBadChar.Length; i++)
            {
                if (arrBadChar[i].Length > 0)
                    tempChar = tempChar.Replace(arrBadChar[i], "");
            }
            return tempChar;
        }
        public static string ReplaceSqlChar(string Text)
        {
            string ReText = Text;
            if (ReText != null && ReText != "")
            {
                ReText = ReText.ToLower();
                ReText = ReText.Replace("'", "''");
                return ReText.Trim();
            }
            else
            {
                return "";
            }
        }

        public static string InputText(string text, int maxLength)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);
            text = Regex.Replace(text, "[\\s]{2,}", " ");    //two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");    //<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");    //&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);    //any other tags
            text = text.Replace("'", "''");
            return text;
        }
        public static string InputText(string text)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            text = Regex.Replace(text, "[\\s]{2,}", " ");    //two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");    //<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");    //&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);    //any other tags
            text = text.Replace("'", "''");
            return text;
        }

        #endregion

        #region 将小写符号转为大写符号
        /// <summary>
        /// 将小写符串转为大写符号 如:将"过滤为“
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceDoubleQuotes(string str)
        {
            str = str.Replace("\"", "“");
            str = str.Replace(",", "，");
            str = str.Replace("'", "‘ ");
            return str;
        }
        #endregion
        #endregion

    }
}
