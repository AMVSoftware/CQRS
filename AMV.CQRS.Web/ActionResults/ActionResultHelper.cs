using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AMV.CQRS.Web.ActionResults
{
    public static class ActionResultHelper
    {
        /// <summary>
        /// Generates a url for the given ActionResult. Used in conjunction with T4MVC.
        /// </summary>
        /// <returns>String with url for the given ActionResult</returns>
        public static String GenerateUrl(this ActionResult actionResult, HtmlHelper htmlHelper)
        {
            var route = actionResult.GetRouteValueDictionary();
            var actionName = (String)route["action"];
            var controllerName = (String)route["controller"];
            var result = UrlHelper.GenerateUrl(null, actionName, controllerName, route, htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, true);
            return result;
        }


        /// <summary>
        /// Generates a url for the given ActionResult. Used in conjunction with T4MVC.
        /// </summary>
        /// <param name="actionResult">ActionResult with parameters to be used for URL generation</param>
        /// <param name="requestContext">If RequestContext is not supplied, `HttpContext.Current.Request.RequestContext` is used by default</param>
        /// <returns>String with url for the given ActionResult</returns>
        public static String GenerateUrl(this ActionResult actionResult, RequestContext requestContext = null)
        {
            if (requestContext == null)
            {
                requestContext = HttpContext.Current.Request.RequestContext;
            }

            var route = actionResult.GetRouteValueDictionary();
            var actionName = (String)route["action"];
            var controllerName = (String)route["controller"];
            var result = UrlHelper.GenerateUrl(null, actionName, controllerName, route, RouteTable.Routes, requestContext, true);
            return result;
        }
    }
}
