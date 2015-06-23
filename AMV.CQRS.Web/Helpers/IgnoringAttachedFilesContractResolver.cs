using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace AMV.CQRS
{
    /// <summary>
    /// Json.Net helper that can serialise the HTTP requests and does not choke on PostedFiles 
    /// </summary>
    public class IgnoringAttachedFilesContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (typeof(Stream).IsAssignableFrom(property.PropertyType))
            {
                property.Ignored = true;
            }
            if (typeof(IEnumerable<HttpPostedFileBase>).IsAssignableFrom(property.PropertyType))
            {
                property.Ignored = true;
            }
            if (typeof(HttpPostedFileBase).IsAssignableFrom(property.PropertyType))
            {
                property.Ignored = true;
            }

            return property;
        }
    }
}