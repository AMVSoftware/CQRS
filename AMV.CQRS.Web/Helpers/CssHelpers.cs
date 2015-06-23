using System;
using System.Text.RegularExpressions;


namespace AMV.CQRS
{
    public static class CssHelpers
    {
        /// <summary>
        /// Clean string from specail characters and prepare to be used as a CSS class name.
        /// Replaces all non-alpha characters with underscore
        /// </summary>
        /// <param name="cssClass"></param>
        /// <returns></returns>
        public static string CleanseCssClassName(string cssClass)
        {
            if (cssClass == null)
            {
                return String.Empty;
            }

            var cleanedClass = Regex.Replace(cssClass, "[^a-zA-Z]", "_");
            return cleanedClass;
        }
    }
}
