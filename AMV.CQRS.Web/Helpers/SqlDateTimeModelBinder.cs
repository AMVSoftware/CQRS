using System;
using System.Web.Mvc;
// ReSharper disable CheckNamespace


namespace AMV.CQRS
{
    /// <summary>
    /// Date-Time model binder that checks if dates are out of range of SQL Server
    /// </summary>
    public class SqlDateTimeModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var result = base.BindModel(controllerContext, bindingContext);

            var dateTime = result as DateTime?;

            if (dateTime != null)
            {
                if (dateTime < System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                {
                    var message = String.Format("{0} must be not less than 01/01/1753", bindingContext.ModelName);
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, message);
                }
                if (dateTime > System.Data.SqlTypes.SqlDateTime.MaxValue.Value)
                {
                    var message = String.Format("{0} must be not more than 31/12/9999", bindingContext.ModelName);
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, message);
                }
            }
            return dateTime;
        }
    }
}