using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
// ReSharper disable CheckNamespace


namespace AMV.CQRS
{
    /// <summary>
    /// Json Result that is aimed to replace default MVC.JsonResult
    /// Uses Json.Net to serialise objects and uses proper date-time format for dates serialisation, 
    /// so when the dates are deserialised in JS on a client there are no issues with timezones
    /// </summary>
    public class CustomJsonResult : JsonResult
    {
        private const string DateFormat = "yyyy-MM-dd HH:mm:ss";

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                // Using Json.NET serializer
                var isoConvert = new IsoDateTimeConverter();
                isoConvert.DateTimeFormat = DateFormat;
                response.Write(JsonConvert.SerializeObject(Data, isoConvert));
            }
        }
    }
}
