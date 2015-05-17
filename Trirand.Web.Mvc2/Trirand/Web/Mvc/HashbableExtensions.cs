namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    internal static class HashbableExtensions
    {
        internal static void AddIfCondition(this Hashtable hash, bool condition, string key, object value)
        {
            if (condition)
            {
                hash.Add(key, value);
            }
        }

        internal static void AddIfNotEmpty(this Hashtable hash, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                hash.Add(key, value);
            }
        }

        internal static void AddIfNotEmpty(this Hashtable hash, string key, string value, Hashtable savedValues)
        {
            if (!string.IsNullOrEmpty(value))
            {
                hash.Add(key, value);
                savedValues.Add(key, value);
            }
        }

        public static void AddLiteral(this Hashtable h, string key, string value, JQChart chart)
        {
            h.Add(key, value);
            chart.AddJsonReplacement(string.Format("\"{0}\"", value), value);
        }
    }
}

