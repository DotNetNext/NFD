namespace Trirand.Web.Mvc
{
    using System;
    using System.Runtime.CompilerServices;

    internal static class DateTimeExtensions
    {
        public static string ToJsonUTC(this DateTime dateTime)
        {
            return string.Format("Date.UTC({0},{1},{2},{3},{4},{5},{6})", new object[] { dateTime.Year, dateTime.Month - 1, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond });
        }
    }
}

