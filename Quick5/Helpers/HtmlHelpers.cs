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
            // new List<Quick5.Models.Siren>() { Model.Siren }
            return new List<T>() { item };
        }
    }
}