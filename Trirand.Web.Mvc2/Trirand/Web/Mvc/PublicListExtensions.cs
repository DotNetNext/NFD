namespace Trirand.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class PublicListExtensions
    {
        public static List<ChartPoint> FromCollection(this List<ChartPoint> points, ICollection collection)
        {
            List<ChartPoint> list = new List<ChartPoint>();
            foreach (object obj2 in collection)
            {
                list.Add(new ChartPoint(obj2));
            }
            return list;
        }
    }
}

