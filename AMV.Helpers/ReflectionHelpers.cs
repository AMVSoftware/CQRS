using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AMV.Helpers
{
    public static class ReflectionHelpers
    {
        /// <summary>
        /// Returns string value of the provided selector function.
        /// I.e. will return "Product" on `order.NameOf(o => o.Product)`
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propSelector"></param>
        /// <returns></returns>
        public static string NameOf<T, TResult>(this T obj, Expression<Func<T, TResult>> propSelector)
        {
            var exp = (MemberExpression)propSelector.Body;
            return exp.Member.Name;
        }


        /// <summary>
        /// Returns string value of the provided selector function.
        /// I.e. will return "Product" on `NameOf<Order,Product>(o => o.Product)`
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TP"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string NameOf<T, TP>(Expression<Func<T, TP>> action) where T : class
        {
            var expression = (MemberExpression)action.Body;
            return expression.Member.Name;
        }


        /// <summary>
        /// Uses reflection to examine member of a class to get out a Display/Description attribute.
        /// Of if no attributes are available, takes the name of the member and converts it to a 
        /// human-readable string.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static String GetDescription(this MemberInfo member)
        {
            if (member == null)
            {
                return String.Empty;
            }

            // first try Display Attribute on the enum
            var attribs = member.GetCustomAttributes(typeof(DisplayAttribute), true);
            if (attribs.Length > 0)
            {
                var displayName = ((DisplayAttribute)attribs[0]).GetName();
                if (!String.IsNullOrWhiteSpace(displayName))
                {
                    return displayName;
                }
            }

            // if Display attribute is not present, try description
            attribs = member.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attribs.Length > 0)
            {
                var description = ((DescriptionAttribute)attribs[0]).Description;
                if (!String.IsNullOrWhiteSpace(description))
                {
                    return description;
                }
            }

            return member.Name.ToSeparatedWords().LowerCasePrepositions();
        }


        public static string GetDescription<TParameter, TValue>(this TParameter parameter,
            Expression<Func<TParameter, TValue>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                return String.Empty;
            }

            var description = memberExpression.Member.GetDescription();

            return description;
        }


        /// <summary>
        /// Examines provided assembly and returns a list of Types that implement the provided type
        /// </summary>
        /// <param name="assembly">Assembly to be analysed</param>
        /// <param name="parentType">Type to be scanned by</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesInheritingFromType(this Assembly assembly, Type parentType)
        {
            var implementingTypes = assembly.GetTypes()
                          .Where(a => a != parentType && parentType.IsAssignableFrom(a))
                          .ToList();

            return implementingTypes;
        }


        /// <summary>
        /// Examines all the assemblies and returns list of all types impelemtning the provided type
        /// </summary>
        /// <param name="assemblies">Collection of assemblies to scan for types</param>
        /// <param name="parentType">Type to be scanned by</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesInheritingFromType(this IEnumerable<Assembly> assemblies, Type parentType)
        {
            var implementingTypes = assemblies.SelectMany(a => a.GetTypes())
                          .Where(a => a != parentType && parentType.IsAssignableFrom(a))
                          .ToList();

            return implementingTypes;
        }
    }
}
