using System;

namespace AMV.Helpers
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Given an enum value, produce a human-readable interpretation of enum.
        /// First looks into DisplayAttribute - returns that if the value is decorated.
        /// If not, returns the text representation of the value, but with spaces between words.
        /// </summary>
        public static string GetDisplayName(this Enum value)
        {
            if (value == null)
            {
                return String.Empty;
            }

            var field = value.GetType().GetField(value.ToString());

            return field.GetDescription();
        }
    }
}
