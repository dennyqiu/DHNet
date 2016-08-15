using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;

namespace DHNet.Components.Extensions.Html
{
    public static class ContentExtensions
    {
        private static Dictionary<String, MvcHtmlString> Scripts = new Dictionary<String, MvcHtmlString>();
        private static Dictionary<String, MvcHtmlString> Styles = new Dictionary<String, MvcHtmlString>();
        public static readonly Int64 Version = DateTime.Now.Ticks;

        public static MvcHtmlString RenderControllerScript(this HtmlHelper html)
        {
            RouteValueDictionary routeValues = html.ViewContext.RouteData.Values;
            String controller = routeValues["controller"].ToString();
            String path = controller;

            if (routeValues["Area"] != null)
                path = routeValues["Area"] + "/" + path;

            if (Scripts.ContainsKey(path))
                return Scripts[path];

            return Scripts[path] = GenerateScriptLink(path, controller, html);
        }
        public static MvcHtmlString RenderControllerStyle(this HtmlHelper html)
        {
            RouteValueDictionary routeValues = html.ViewContext.RouteData.Values;
            String controller = routeValues["controller"].ToString();
            String path = controller;

            if (routeValues["Area"] != null)
                path = routeValues["Area"] + "/" + path;

            if (Styles.ContainsKey(path))
                return Styles[path];

            return Styles[path] = GenerateStyleLink(path, controller, html);
        }

        private static MvcHtmlString GenerateScriptLink(String path, String controller, HtmlHelper html)
        {
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            String virtualPath = urlHelper.Content(String.Format("~/Scripts/Shared/{0}/{1}.js", path, controller.ToLower()));
            String physicalPath = html.ViewContext.HttpContext.Server.MapPath(virtualPath);
            if (!File.Exists(physicalPath)) return MvcHtmlString.Empty;

            TagBuilder script = new TagBuilder("script");
            script.MergeAttribute("src", virtualPath + "?v" + Version);

            return new MvcHtmlString(script.ToString());
        }
        private static MvcHtmlString GenerateStyleLink(String path, String controller, HtmlHelper html)
        {
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            String virtualPath = urlHelper.Content(String.Format("~/Content/Shared/{0}/{1}.css", path, controller.ToLower()));
            String physicalPath = html.ViewContext.HttpContext.Server.MapPath(virtualPath);
            if (!File.Exists(physicalPath)) return MvcHtmlString.Empty;

            TagBuilder link = new TagBuilder("link");
            link.MergeAttribute("href", virtualPath + "?v" + Version);
            link.MergeAttribute("rel", "stylesheet");

            return new MvcHtmlString(link.ToString());
        }
    }
}
