using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AMV.Helpers;


namespace AMV.CQRS
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Create a list with SelectListItems from an Enum.
        /// Takes DisplayAttribues first if present, otherwise takes string value of enum and separates with spaces words.
        /// </summary>
        public static List<SelectListItem> ToSelectListItems(Type enumType)
        {
            if (enumType.BaseType != typeof(Enum))
            {
                throw new NotSupportedException("Unable to generate SelectList for non-enum");
            }
            var items = ConstructItemsList(enumType);

            return items.Select(pair => new SelectListItem()
            {
                Text = pair.Value,
                Value = pair.Key.ToString(),
            })
            .ToList();
        }

        /// <summary>
        /// Create a list with SelectListItems from an Enum.
        /// Takes DisplayAttribues first if present, otherwise takes string value of enum and separates with spaces words.
        /// </summary>
        public static List<SelectListItem> ToSelectListItems(this Enum @enum)
        {
            var items = ConstructItemsList(@enum.GetType());

            return items.Select(pair => new SelectListItem()
            {
                Text = pair.Value,
                Value = pair.Key.ToString(),
                Selected = (pair.Value == @enum.ToString())
            }).ToList();
        }


        private static Dictionary<object, string> ConstructItemsList(Type enumType)
        {
            var source = Enum.GetValues(enumType);
            var items = new Dictionary<object, string>();

            foreach (var value in source)
            {
                var field = value.GetType().GetField(value.ToString());

                if (field == null)
                {
                    continue;
                }

                var description = field.GetDescription();
                items.Add((int)value, description);
            }

            return items;
        }
    }
}