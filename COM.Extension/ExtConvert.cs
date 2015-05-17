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
using System.Globalization;

namespace COM.Extension
{
    /// <summary>     
    /// 1. 功能：数据转换
    /// 2. 作者：kaixuan
    /// 3. 创建日期：2014-10-24
    /// 4. 最后修改日期：2014-10-24
    /// </summary>    
    public static class ExtConvert
    {

        #region 强转成int 如果失败返回 0
        /// <summary>
        /// 强转成int 如果失败返回 0
        /// </summary>
        /// <param name="o"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int ToInt(this object o)
        {
            if (o == null || o == DBNull.Value) return 0;
            else if (o.ToString() == "") return 0;
            else
                return Convert.ToInt32(o);
        }
        #endregion
        #region 强转成int 如果失败返回 i
        /// <summary>
        /// 强转成int 如果失败返回 i
        /// </summary>
        /// <param name="o"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int ToInt(this object o, int i)
        {
            int reval = 0;
            if (o != null && int.TryParse(o.ToString(), out reval))
            {
                return reval;
            }
            return i;
        }
        #endregion
        #region 强转成double 如果失败返回 0
        /// <summary>
        /// 强转成double 如果失败返回 0
        /// </summary>
        /// <param name="o"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static double ToDou(this object o)
        {
            double reval = 0;
            if (o != null && double.TryParse(o.ToString(), out reval))
            {
                return reval;
            }
            return 0;
        }
        #endregion
        #region 强转成double 如果失败返回 i
        /// <summary>
        /// 强转成double 如果失败返回 i
        /// </summary>
        /// <param name="o"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static double ToDou(this object o, int i)
        {
            double reval = 0;
            if (o != null && double.TryParse(o.ToString(), out reval))
            {
                return reval;
            }
            return i;
        }
        #endregion
        #region 强转成string 如果失败返回 ""
        /// <summary>
        /// 强转成string 如果失败返回 ""
        /// </summary>
        /// <param name="o"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string ToConvertString(this object o)
        {
            if (o != null) return o.ToString().Trim();
            return "";
        }
        #endregion
        #region  强转成string 如果失败返回 str
        /// <summary>
        /// 强转成string 如果失败返回 str
        /// </summary>
        /// <param name="o"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToConvertString(this object o, string str)
        {
            if (o != null) return o.ToString().Trim();
            return str;
        }
        #endregion
        #region  强转成 decimal
        /// <summary>
        /// 强转成 decimal
        /// </summary>
        /// <param name="o"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this decimal? o)
        {
            if (o != null) return Convert.ToDecimal(o);
            return 0;
        }
        #endregion
        #region  强转成 decimal并且格式化
        /// <summary>
        /// 强转成 decimal并且格式化
        /// </summary>
        /// <param name="dec"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToDecimal(this decimal? dec,string format)
        {
            if (dec != null) return Convert.ToDecimal(dec).ToString(format);
            return "0";
        }
        #endregion
        #region 强转为时间格式
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="o"></param>
        /// <param name="FormatStr"></param>
        /// <returns></returns>
        public static string ToDateStr(this object o, string FormatStr)
        {
            if (o == null) return "";
            return Convert.ToDateTime(o).ToString(FormatStr);
        }
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="o"></param>
        /// <param name="FormatStr"></param>
        /// <returns></returns>
        public static DateTime? ToDate(this string o)
        {
            DateTime reval = DateTime.Now;
            if (o != null && DateTime.TryParse(o.ToString(), out reval))
            {
                return reval;
            }
            return null;
        }
        #endregion
        #region 将匿名对象转为 SqlParameter【】
        /// <summary>
        /// 将匿名对象转成 SqlParameter[] 
        /// </summary>
        /// <param name="o">如 var o=new {id=1,name="张三"}</param>
        /// <returns>SqlParameter[]</returns>
        public static SqlParameter[] ToPars(this object o)
        {
            List<SqlParameter> pars = new List<SqlParameter>();
            RouteValueDictionary rvd = new RouteValueDictionary(o);
            foreach (var r in rvd)
            {
                SqlParameter par;
                if (r.Value == null)
                {
                    par = new SqlParameter("@" + r.Key, DBNull.Value);
                }
                else
                {
                    par = new SqlParameter("@" + r.Key, r.Value);
                }
                pars.Add(par);
            }
            return pars.ToArray();
        }
        #endregion
        #region 将数据转换为指定类型
        /// <summary>
        /// 将数据转换为指定类型
        /// </summary>
        /// <param name="data">转换的数据</param>
        /// <param name="targetType">转换的目标类型</param>
        public static object ConvertToByType(this object data, Type targetType)
        {
            //如果数据为空，则返回
            if (data == null)
            {
                return null;
            }

            try
            {
                //如果数据实现了IConvertible接口，则转换类型
                if (data is IConvertible)
                {
                    return Convert.ChangeType(data, targetType);
                }
                else
                {
                    return data;
                }
            }
            catch
            {
                return null;
            }
        } 
        #endregion

        #region json转换
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static TEntity ToModel<TEntity>(this string json)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Deserialize<TEntity>(json);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(obj);
        }

        #endregion

        #region 对像转换
        /// <summary>
        /// 将对像转为匿名类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static dynamic ToDynamic<T>(this List<T> obj)
        {
            if (obj == null || obj.Count == 0) return null;
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(obj).ToModel<dynamic>();
        }

        /// <summary>
        /// 将对像转为匿名类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(this object obj)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(obj).ToModel<dynamic>();
        }

        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = typeof(T).GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 将datatable转为list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt)
        {
            var list = new List<T>();
            Type t = typeof(T);
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            foreach (DataRow item in dt.Rows)
            {
                T s = System.Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        if (!Convert.IsDBNull(item[i]))
                        {
                            info.SetValue(s, item[i], null);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }

        #endregion

        #region 数组
        /// <summary>
        /// 将数组以豆号格开
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToJoin(this object[] array)
        {
            if (array == null || array.Length == 0)
            {
                return "";
            }
            else
            {
                return string.Join(",", array.Where(c => c != null).Select(c => c.ToString().Trim()));
            }
        }
        /// <summary>
        /// 将数组转为 '1','2' 这种格式的字符串 用于 where id in(  )
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToJoinSqlInVal(this object[] array)
        {
            if (array == null || array.Length == 0)
            {
                return "";
            }
            else
            {
                return string.Join(",", array.Where(c => c != null).Select(c => "'" + c.ToSqlFilter() + "'"));//除止SQL注入
            }
        }
        /// <summary>
        /// 将数组转为 '1','2' 这种格式的字符串 用于 where id in(  )
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToJoinSqlInVal(this Guid[] array)
        {
            if (array == null || array.Length == 0)
            {
                return "";
            }
            else
            {
                return string.Join(",", array.Where(c => c != null).Select(c => "'" + c.ToSqlFilter() + "'"));//除止SQL注入
            }
        }
        #endregion

        #region 金钱
        /// <summary>
        /// 把数字转换成对应的大写钱数
        /// </summary>
        /// <param name="strDigital">要转换的数字</param>
        /// <param name="isCapital">默认为大写，设为false则为小写</param>
        /// <returns>转换结果</returns>
        public static string ToSumToCap(this string strDigital, bool isCapital = true)
        {
            decimal dshuzi;
            string ret = "";
            dshuzi = decimal.Parse(strDigital);

            if (dshuzi == 0)
            {
                ret = "零元整";
                return ret;
            }

            string je = dshuzi.ToString("####.00");
            if (je.Length > 15)
                return "";
            je = new String('0', 15 - je.Length) + je;//若小于15位长，前面补0

            string stry = je.Substring(0, 4);//取得'亿'单元
            string strw = je.Substring(4, 4);//取得'万'单元
            string strg = je.Substring(8, 4);//取得'元'单元
            string strf = je.Substring(13, 2);//取得小数部分

            string str1 = "", str2 = "", str3 = "";

            str1 = stry.ToMoneyCapitalAndUnit("亿");//亿单元的大写
            str2 = strw.ToMoneyCapitalAndUnit("万");//万单元的大写
            str3 = strg.ToMoneyCapitalAndUnit("元");//元单元的大写

            string str_y = "", str_w = "";
            if (je[3] == '0' || je[4] == '0')//亿和万之间是否有0
                str_y = "零";
            if (je[7] == '0' || je[8] == '0')//万和元之间是否有0
                str_w = "零";

            ret = str1 + str_y + str2 + str_w + str3;//亿，万，元的三个大写合并

            for (int i = 0; i < ret.Length; i++)//去掉前面的"零"
            {
                if (ret[i] != '零')
                {
                    ret = ret.Substring(i);
                    break;
                }

            }
            for (int i = ret.Length - 1; i > -1; i--)//去掉最后的"零"
            {
                if (ret[i] != '零')
                {
                    ret = ret.Substring(0, i + 1);
                    break;
                }
            }

            if (ret[ret.Length - 1] != '元')//若最后不位不是'元'，则加一个'元'字
                ret = ret + "元";

            if (ret == "零零元")//若为零元，则去掉"元数"，结果只要小数部分
                ret = "";

            if (strf == "00")//下面是小数部分的转换
            {
                ret = ret + "整";
            }
            else
            {
                string tmp = "";
                tmp = strf[0].ToMoneyCapital();
                if (tmp == "零")
                    ret = ret + tmp;
                else
                    ret = ret + tmp + "角";

                tmp = strf[1].ToMoneyCapital();
                if (tmp == "零")
                    ret = ret + "整";
                else
                    ret = ret + tmp + "分";
            }

            if (ret[0] == '零')
            {
                ret = ret.Substring(1);//防止0.03转为"零叁分"，而直接转为"叁分"
            }
            if (!isCapital)
            {
                ret = ret.Replace("零", "零");
                ret = ret.Replace("壹", "一");
                ret = ret.Replace("贰", "二");
                ret = ret.Replace("叁", "三");
                ret = ret.Replace("肆", "四");
                ret = ret.Replace("伍", "五");
                ret = ret.Replace("陆", "六");
                ret = ret.Replace("柒", "七");
                ret = ret.Replace("拐", "八");
                ret = ret.Replace("玖", "九");
                ret = ret.Replace("拾", "十");
                ret = ret.Replace("仟", "千");
                ret = ret.Replace("佰", "百");

            }
            return ret;//完成，返回
        }

        /// <summary>
        /// 将金钱加单位    
        /// 比如:  str=1200 unit=万 结果:1200万 
        /// 调用："1200".ToMoneyCapitalAndUnit("万")
        /// </summary>
        /// <param name="str">这个单元的小写数字（4位长，若不足，则前面补零）</param>
        /// <param name="unit">亿，万，元</param>
        /// <returns>转换结果</returns>
        public static string ToMoneyCapitalAndUnit(this string str, string unit)
        {
            if (str == "0000")
                return "";

            string ret = "";
            string tmp1 = str[0].ToMoneyCapital();
            string tmp2 = str[1].ToMoneyCapital();
            string tmp3 = str[2].ToMoneyCapital();
            string tmp4 = str[3].ToMoneyCapital();
            if (tmp1 != "零")
            {
                ret = ret + tmp1 + "仟";
            }
            else
            {
                ret = ret + tmp1;
            }

            if (tmp2 != "零")
            {
                ret = ret + tmp2 + "佰";
            }
            else
            {
                if (tmp1 != "零")//保证若有两个零'00'，结果只有一个零，下同
                    ret = ret + tmp2;
            }

            if (tmp3 != "零")
            {
                ret = ret + tmp3 + "拾";
            }
            else
            {
                if (tmp2 != "零")
                    ret = ret + tmp3;
            }

            if (tmp4 != "零")
            {
                ret = ret + tmp4;
            }

            if (ret[0] == '零')//若第一个字符是'零'，则去掉
                ret = ret.Substring(1);
            if (ret[ret.Length - 1] == '零')//若最后一个字符是'零'，则去掉
                ret = ret.Substring(0, ret.Length - 1);

            return ret + unit;//加上本单元的单位

        }
        /// <summary>
        /// 单个数字转为大写
        /// </summary>
        /// <param name="c">小写阿拉伯数字 0---9</param>
        /// <returns>大写数字</returns>
        public static string ToMoneyCapital(this char c)
        {
            string str = "";
            switch (c)
            {
                case '0':
                    str = "零";
                    break;
                case '1':
                    str = "壹";
                    break;
                case '2':
                    str = "贰";
                    break;
                case '3':
                    str = "叁";
                    break;
                case '4':
                    str = "肆";
                    break;
                case '5':
                    str = "伍";
                    break;
                case '6':
                    str = "陆";
                    break;
                case '7':
                    str = "柒";
                    break;
                case '8':
                    str = "拐";
                    break;
                case '9':
                    str = "玖";
                    break;
            }
            return str;
        }

        /// <summary>
        /// 将数字转为金钱格式 
        /// 例如:60000转换为60,000.00
        /// </summary>
        /// <param name="o">数字可以有小数</param>
        /// <returns></returns>
        public static string ToMoneyString(this Object o)
        {
            decimal reval = 0;
            decimal.TryParse((o+"").ToString(), out reval);
            return reval.ToString("n2");
        }


        /// <summary>
        /// 将数字转为金钱格式 
        /// 例如:60,000.00转换为60000.00
        /// </summary>
        /// <param name="o">数字可以有小数</param>
        /// <returns></returns>
        public static Double ToMoney(this Object o)
        {
            if (o == null) return 0;
            decimal reval = 0;
            decimal.TryParse((o+"").ToString(), out reval);
            return Convert.ToDouble(reval);
        }
        #endregion

    }
}
