using System.Web.Mvc;
using Newtonsoft.Json;


namespace AMV.CQRS
{
    public static class JsonHelpers
    {
        /// <summary>
        /// Serialise MVC view model into JSON
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static MvcHtmlString ToJson<TModel>(this HtmlHelper htmlHelper, TModel model)
        {
            return new MvcHtmlString(model.ToJson());
        }


        /// <summary>
        /// Serializes an object to Javascript Object Notation.
        /// </summary>
        /// <param name="item">The item to serialize.</param>
        /// <returns>
        /// The item serialized as Json.
        /// </returns>
        public static string ToJson(this object item)
        {
            var jsonObject = JsonConvert.SerializeObject(item, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new IgnoringAttachedFilesContractResolver(),
            });
            return jsonObject;
        }
    }
}