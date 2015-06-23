using System;
using System.Collections;
using System.Web.Routing;

namespace AMV.CQRS
{
    /// <summary>
    /// Way of getting a route value dictionary in a format required by MVC if it contains IEnumerable
    /// See here http://stackoverflow.com/questions/19960420/adding-array-of-complex-types-to-routevaluedictionary
    /// Could also look at using something like this if you are using routes http://vladimirgoncharov.blogspot.co.uk/2013/10/passing-array-as-route-value-within-html-actionlink.html
    /// </summary>
    public static class RouteValueDictionaryExtension
    {
        public static RouteValueDictionary ToRouteValueDictionaryWithCollection(this RouteValueDictionary routeValues)
        {
            var newRouteValues = new RouteValueDictionary();

            foreach (var key in routeValues.Keys)
            {
                object value = routeValues[key];

                if (value is IEnumerable && !(value is string))
                {
                    int index = 0;
                    foreach (object val in (IEnumerable)value)
                    {
                        if (val is string || val.GetType().IsPrimitive)
                        {
                            newRouteValues.Add(String.Format("{0}[{1}]", key, index), val);
                        }
                        else
                        {
                            var properties = val.GetType().GetProperties();
                            foreach (var propInfo in properties)
                            {
                                newRouteValues.Add(
                                    String.Format("{0}[{1}].{2}", key, index, propInfo.Name),
                                    propInfo.GetValue(val));
                            }
                        }
                        index++;
                    }
                }
                else
                {
                    newRouteValues.Add(key, value);
                }
            }

            return newRouteValues;
        }

        public static RouteValueDictionary WithStandardKendoDefaultFilters(this RouteValueDictionary routeValues)
        {
            routeValues.Add("filter", "~");
            routeValues.Add("sort", "~");
            return routeValues;
        }
    }
}