namespace System.Web.Mvc.Html
{
    public static class BootstrapHelpersExtension
    {
        public static BootstrapHelpers<TModel> Bootstrap<TModel>(this HtmlHelper<TModel> helper)
        {
            return new BootstrapHelpers<TModel>(helper);
        }
    }

    public class BootstrapHelpers<TModel>
    {
        private HtmlHelper<TModel> helper { get; set; }

        public BootstrapHelpers(HtmlHelper<TModel> helper)
        {
            this.helper = helper;
        }

        public MvcHtmlString NavLink(string linkText, string actionName, string controllerName, object routeValues)
        {
            var tag = new TagBuilder("li");
            tag.InnerHtml = this.helper.ActionLink(linkText, actionName, controllerName, routeValues, null).ToString();

            var current_controller = this.helper.ViewContext.RouteData.Values["controller"].ToString().ToLower();
            if (controllerName.ToLower() == current_controller)
            {
                var current_action = this.helper.ViewContext.RouteData.Values["action"].ToString().ToLower();
                if (actionName.ToLower() == current_action)
                {
                    tag.AddCssClass("active");
                }
            }

            return new MvcHtmlString(tag.ToString(TagRenderMode.Normal));
        }

        public MvcHtmlString BtnLink(string linkText, string actionName, object routeValues = null)
        {
            var css = "btn btn-xs";
            switch (actionName.ToLower())
            {
                case "delete":
                case "init":
                    css += " btn-danger";
                    break;
                default:
                    css += " btn-default";
                    break;
            }

            var current_action = this.helper.ViewContext.RouteData.Values["action"].ToString().ToLower();
            if (current_action == "index")
            {
                if (actionName.ToLower() == "edit") css += " hidden-sm";
                if (actionName.ToLower() == "delete") css += " hidden-sm";
            }
            else if (actionName.ToLower() == current_action)
            {
                css += " disabled";
            }

            var link = this.helper.ActionLink(linkText, actionName, routeValues, new { @class = css });

            return link;
        }
    }
}