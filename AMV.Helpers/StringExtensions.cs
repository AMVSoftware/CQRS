using System;
using System.Text.RegularExpressions;

namespace AMV.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Insert spaces before capital letter in the string. I.e. "HelloWorld" turns into "Hello World"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSeparatedWords(this string value)
        {
            if (value != null)
            {
                var regex = @"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])";

                var r = new Regex(regex);
                return r.Replace(value, " ").Trim();
            }
            return null;
        }


        /// <summary>
        /// If the string is a whitespace, returns null, otherwise returns the same string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NullIfWhiteSpace(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return value;
        }


        /// <summary>
        /// Inspects the provided string for perpositions and changes the case of these prepositions into a lowercase.
        /// i.e. "Go For A Walk" will become "Go for a Walk"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LowerCasePrepositions(this string value)
        {
            const string prepositions = "As At But By For In Of Off On Onto Per Than To Up Via With A The";
            var tokens = value.Split(' ');
            if (tokens.Length == 1)
            {
                return value;
            }
            // Always leave the first and last word capitalised
            for (var i = 1; i < tokens.Length - 1; i++)
            {
                if (prepositions.Contains(tokens[i]))
                {
                    tokens[i] = tokens[i].ToLower();
                }
            }
            return string.Join(" ", tokens);
        }

        /// <summary>
        /// Extension method providing easy way of doing case insensitive contains test
        /// </summary>
        /// <param name="source">String to be checked</param>
        /// <param name="value">The value to be looked for</param>
        /// <param name="comp">Type of string comparison</param>
        /// <returns>True if string is contained. False if string not found or source is null</returns>
        public static bool Contains(this string source, string value, StringComparison comp)
        {
            if (string.IsNullOrEmpty(source))
            {
                return false;
            }
            return source.IndexOf(value, comp) >= 0;
        }


        /// <summary>
        /// Trim long string into smaller one, finish with "..." and try to cut the string on the word/centence finish
        /// </summary>
        /// <param name="source">String to be shortened</param>
        /// <param name="approximateLength">Length to cut for</param>
        /// <returns></returns>
        public static string Elipsis(this string source, int approximateLength)
        {
            var chars = new[] { '.', '!', ',', ';', ' ' };
            if (string.IsNullOrWhiteSpace(source))
            {
                return string.Empty;
            }

            if (source.Length <= approximateLength)
            {
                return source;
            }

            var defaultLength = source.Substring(0, approximateLength).LastIndexOfAny(chars);
            if (defaultLength == -1 || defaultLength > approximateLength)
            {
                defaultLength = approximateLength;
            }
            var result = source.Substring(0, defaultLength);
            var elipsis = result.Trim() + "...";
            return elipsis;
        }

        /// <summary>
        /// Converts a string to a valid CSS class name
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToValidCssClassName(this string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var validCssClassName = Regex.Replace(value, "[^a-zA-Z]", "_");
            return validCssClassName;
        }



        /// <summary>
        /// Checks if string contains "true", case insensitive
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTrue(this string value)
        {
            return string.Equals(value, "true", StringComparison.InvariantCultureIgnoreCase);
        }
    }

    // ReSharper disable once CheckNamespace
    namespace System
    {
        public static class StringExtensions
        {
            public static bool IsNullOrEmpty(this String @string)
            {
                return String.IsNullOrEmpty(@string);
            }

            public static bool IsNullOrWhiteSpace(this String @string)
            {
                return String.IsNullOrWhiteSpace(@string);
            }

            public static string SafeSubstring(this string text, int start, int length)
            {
                return text.Length <= start ? ""
                    : text.Length - start <= length ? text.Substring(start)
                    : text.Substring(start, length);
            }
        }
    }
}
