namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    internal static class ObjectExtensions
    {
        public static object ToJson(this object value, JQChart chart)
        {
            if ((value != null) && (value is DateTime))
            {
                string str = ((DateTime) value).ToJsonUTC();
                chart.AddJsonReplacement(string.Format("\"{0}\"", str), str);
                return str;
            }
            return value;
        }
    }
}

