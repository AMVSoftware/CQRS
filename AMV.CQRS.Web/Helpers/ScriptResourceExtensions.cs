using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;


namespace AMV.CQRS
{
    /// <summary>
    /// Courtesy: http://stackoverflow.com/a/5433722/476786
    /// </summary>
    public static class ScriptResourceExtensions
    {
        private const string Js = "js";
        private const string Css = "css";

        public static IHtmlString SectionScript(this HtmlHelper htmlHelper, Func<object, HelperResult> template)
        {
            if (htmlHelper.ViewContext.HttpContext.Items[Js] != null)
            {
                ((List<Func<object, HelperResult>>)htmlHelper.ViewContext.HttpContext.Items[Js]).Add(template);
            }
            else
            {
                htmlHelper.ViewContext.HttpContext.Items[Js] = new List<Func<object, HelperResult>> { template };
            }

            return new HtmlString(String.Empty);
        }

        public static IHtmlString SectionStyle(this HtmlHelper htmlHelper, Func<object, HelperResult> template)
        {
            if (htmlHelper.ViewContext.HttpContext.Items[Css] != null)
            {
                ((List<Func<object, HelperResult>>)htmlHelper.ViewContext.HttpContext.Items[Css]).Add(template);
            }
            else
            {
                htmlHelper.ViewContext.HttpContext.Items[Css] = new List<Func<object, HelperResult>> { template };
            }

            return new HtmlString(String.Empty);
        }

        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.ViewContext.HttpContext.Items[Js] != null)
            {
                var resources = (List<Func<object, HelperResult>>)htmlHelper.ViewContext.HttpContext.Items[Js];

                foreach (var resource in resources.Where(resource => resource != null))
                {
                    htmlHelper.ViewContext.Writer.Write(resource(null));
                }
            }

            return new HtmlString(String.Empty);
        }

        public static IHtmlString RenderStyles(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.ViewContext.HttpContext.Items[Css] != null)
            {
                var resources = (List<Func<object, HelperResult>>)htmlHelper.ViewContext.HttpContext.Items[Css];

                foreach (var resource in resources.Where(resource => resource != null))
                {
                    htmlHelper.ViewContext.Writer.Write(resource(null));
                }
            }

            return new HtmlString(String.Empty);
        }
    }
}
