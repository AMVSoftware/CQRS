using System;
using System.Globalization;

namespace AMV.Helpers
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Checks the provided @object to be null.
        /// If the @object is not null, executes provided functions and return a result of the execution.
        /// If the @object is null, then return empty string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <param name="func"></param>
        /// <returns>Result of executed function</returns>
        public static String CheckForNull<T>(this T @object, Func<T, String> func)
        {
            if (@object == null)
            {
                // default(String) returns null, but we need empty string
                return String.Empty;
            }

            return func.Invoke(@object);
        }


        /// <summary>
        /// Executes function on the object if the provided object is not null.
        /// Returns the result of the execution of the function. 
        /// Or in case when object is null, then returns default value for type of TResult
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="object"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult CheckForNull<T, TResult>(this T @object, Func<T, TResult> func)
        {
            if (@object == null)
            {
                return default(TResult);
            }

            return func.Invoke(@object);
        }



        /// <summary>
        /// Executes function on the object if the provided object is not null.
        /// Returns the result of the execution of the function. 
        /// Or in case when object is null, then returns the provided default value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="object"></param>
        /// <param name="func">Function to be executed if @object is not null</param>
        /// <param name="defaultResult">Value to be returned if param @object is null</param>
        /// <returns></returns>
        public static TResult CheckForNull<T, TResult>(this T @object, Func<T, TResult> func, TResult defaultResult)
        {
            if (@object == null)
            {
                return defaultResult;
            }

            return func.Invoke(@object);
        }


        /// <summary>
        /// Execute Action on @object if @object is not null. 
        /// If provided @object is null, do nothing. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <param name="func"></param>
        public static void IfNotNull<T>(this T @object, Action<T> func)
        {
            if (@object == null)
            {
                return;
            }

            func.Invoke(@object);
        }



        /// <summary>
        /// Check if examined @object represents numeric value or not.
        /// </summary>
        /// <param name="object"></param>
        /// <returns>True if object is of numeric nature</returns>
        public static bool IsNumeric(this object @object)
        {
            if (@object == null)
            {
                return false;
            }

            double number;
            
            var stringyValue = Convert.ToString(@object, CultureInfo.InvariantCulture);

            var isNumeric = Double.TryParse(stringyValue, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out number);
            return isNumeric;
        }
    }
}
