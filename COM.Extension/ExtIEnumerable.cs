using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Reflection;

namespace COM.Extension
{
    /// <summary>     
    /// 1. 功能：IEnumerable扩展功能
    /// 2. 作者：kaixuan
    /// 3. 创建日期：2014-10-24
    /// 4. 最后修改日期：2014-10-24
    /// </summary>    
    public static class ExtIEnumerable
    {
        #region IEnumerable通用扩展方法
        /// <summary>
        /// IEnumerable通用扩展方法
        /// </summary>     
        public static void FillEach<T>(this IEnumerable<T> List, Action<T> Action)
        {
            if (List != null && List.Count() > 0 && Action != null)
            {
                foreach (T item in List)
                {
                    Action.Invoke(item);
                }
            }
        }
        /// <summary>
        /// IEnumerable通用扩展方法
        /// </summary>     
        public static void FillEach<T>(this List<T> List, Action<T> Action)
        {
            if (List != null && List.Count > 0 && Action != null)
            {
                foreach (T item in List)
                {
                    Action.Invoke(item);
                }
            }
        }
        /// <summary>
        /// 添加 并且新添元素在索引第一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<T> AddFirstItem<T>(this IEnumerable<T> list, T t)
        {
            List<T> reval = new List<T>();
            reval.Add(t);
            reval.AddRange(list);
            return reval;
        }
        public static void For<T>(this IEnumerable<T> List, int Start, int End, int Step, Action<T> Action)
        {
            for (int i = Start; i < End; i = i + Step)
            {
                Action.Invoke(List.ElementAt(i));
            }
        }
        public static void For<T>(this IEnumerable<T> List, int Start, Func<int, bool> End, int Step, Action<T> Action)
        {
            for (int i = Start; End.Invoke(i); i = i + Step)
            {
                Action.Invoke(List.ElementAt(i));
            }
        }
        public static void For<T>(this IEnumerable<T> List, int Start, Func<int, bool> End, int Step, Func<T, int, int> Action)
        {
            for (int i = Start; End.Invoke(i); i = i + Step)
            {
                i = Action.Invoke(List.ElementAt(i), i);
            }
        }
        public static List<int> RangeFormTo(this List<int> o, int begin, int end)
        {
            List<int> list = new List<int>();
            for (int i = begin; i <= end; i++)
            {
                list.Add(i);
            }
            return list;
        }


        #endregion
    }
}
