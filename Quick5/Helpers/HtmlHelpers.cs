using System.Collections.Generic;
using Quick5;

namespace System.Web.Mvc
{
    public static class HtmlHelpers
    {
        public static bool IsDbProduction(this HtmlHelper helper)
        {
            return MvcApplication.IsDbProduction;
        }

        public static List<T> AsList<T>(this T item)
        {
            return new List<T>() { item };
        }

        public static string ToShort(this DateTime? dt)
        {
            var value = dt.GetValueOrDefault();
            if (value == DateTime.MinValue) return "";
            if (value == DateTime.MaxValue) return "∞";
            if (value == new DateTime(4712, 12, 31)) return "∞";

            return value.ToString("dd/MM/yy");
        }
    }
}