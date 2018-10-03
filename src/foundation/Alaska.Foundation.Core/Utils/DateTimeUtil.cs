using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alaska.Foundation.Core.Utils
{
    public static class DateTimeUtil
    {
        public static DateTime ParseIsoDate(string date)
        {
            return DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static string FormatIsoDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}
