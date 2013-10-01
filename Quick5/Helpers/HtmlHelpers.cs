using Quick5;

namespace System.Web.Mvc
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString DbWarning(this HtmlHelper helper)
        {
            var warning = MvcApplication.IsDbProduction ? " - ATTENTION // BASE DE DONNEES DE PRODUCTION" : "";

            return new MvcHtmlString(warning);
        }
    }
}