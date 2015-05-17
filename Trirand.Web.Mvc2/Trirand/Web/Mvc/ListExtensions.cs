namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal static class ListExtensions
    {
        public static string ToJSON(this List<ChartXAxisSettings> settings, JQChart chart)
        {
            string str = "";
            int num = 0;
            foreach (ChartXAxisSettings settings2 in settings)
            {
                if (num > 0)
                {
                    str = str + ",";
                }
                str = str + settings2.ToJSON(chart);
                num++;
            }
            if (num > 1)
            {
                str = "[" + str + "]";
            }
            return str;
        }

        public static string ToJSON(this List<ChartYAxisSettings> settings, JQChart chart)
        {
            string str = "";
            int num = 0;
            foreach (ChartYAxisSettings settings2 in settings)
            {
                if (num > 0)
                {
                    str = str + ",";
                }
                str = str + settings2.ToJSON(chart);
                num++;
            }
            if (num > 1)
            {
                str = "[" + str + "]";
            }
            return str;
        }
    }
}

