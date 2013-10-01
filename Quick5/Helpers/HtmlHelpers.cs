using Quick5;

namespace System.Web.Mvc
{
    public static class HtmlHelpers
    {
        public static bool IsDbProduction(this HtmlHelper helper)
        {
            return MvcApplication.IsDbProduction;
        }
    }
}