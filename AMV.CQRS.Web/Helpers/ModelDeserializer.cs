using System;
using System.Globalization;
using System.Reflection;
using System.Web.Routing;
using System.Collections;


namespace AMV.CQRS
{
    //http://dukelupus.wordpress.com/2011/04/26/simple-asp-net-mvc-object-serializer/
    public class ModelDeserializer
    {
        private readonly RouteValueDictionary dictionary = new RouteValueDictionary();

        /// <summary>
        /// Recursive method
        /// </summary>
        public RouteValueDictionary ToRouteDictionary(object o, string prefix = "")
        {
            var properties = o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.PropertyType.IsValueType || property.PropertyType.Name == "String")
                {
                    var value = property.GetValue(o, BindingFlags.Public, null, null, CultureInfo.CurrentCulture);
                    var defValue = property.PropertyType.IsValueType
                                       ? Activator.CreateInstance(property.PropertyType)
                                       : null;
                    //we don't need default values
                    if (value != null && !value.Equals(defValue))
                    {
                        dictionary[string.Format("{0}{1}", prefix, property.Name)] = value;
                    }
                    continue;
                }
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    var index = 0;
                    var value = property.GetValue(o, BindingFlags.Public, null, null, CultureInfo.CurrentCulture);

                    var vals = (IEnumerable)value;
                    if (vals == null)
                    {
                        continue;
                    }
                    foreach (var val in vals)
                    {
                        if (val is string || val.GetType().IsPrimitive)
                        {
                            dictionary.Add(String.Format("{0}[{1}]", string.Concat(prefix, property.Name), index), val);
                        }
                        else
                        {
                            var concat = string.Concat(prefix, property.Name + "[" + index + "].");
                            ToRouteDictionary(val, concat);
                        }
                        index++;
                    }
                    continue;
                }
                ToRouteDictionary(property.GetValue(o, null), string.Concat(prefix, property.Name, "."));
            }
            return dictionary;
        }
    }
}