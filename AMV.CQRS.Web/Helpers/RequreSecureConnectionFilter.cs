using System;
using System.Web.Mvc;
// ReSharper disable CheckNamespace


namespace AMV.CQRS
{
    /// <summary>
    /// Secure Connection filter: same as default MVC RequireHttps filter, 
    /// but does not require SSL on local requests
    /// </summary>
    public class RequreSecureConnectionFilter : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.HttpContext.Request.IsLocal)
            {
                return;
            }
            
            base.OnAuthorization(filterContext);
        }
    }
}