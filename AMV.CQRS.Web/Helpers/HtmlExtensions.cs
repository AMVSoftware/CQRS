using System;
using System.Web.Mvc;


namespace AMV.CQRS
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Value not always preserved in Model, sometimes when we submit the form, 
        /// validator has failed and we RestoreModelState, correct value that we want to show is contained in ModelState.
        /// This extracts data from 
        /// </summary>
        public static TValue GetElementValue<T, TValue>(this HtmlHelper<T> html, ModelMetadata metadata)
        {
            if (!String.IsNullOrEmpty(metadata.PropertyName) && html.ViewData.ModelState.ContainsKey(metadata.PropertyName))
            {
                var modelState = html.ViewData.ModelState[metadata.PropertyName];
                var value = modelState.Value;
                if (value != null && value.AttemptedValue != null && !String.IsNullOrEmpty(value.AttemptedValue))
                {
                    try
                    {
                        var date = (TValue)value.ConvertTo(typeof(TValue));
                        return date;
                    }
                    catch (Exception)
                    {
                        return default(TValue);
                    }
                }
            }

            if (metadata.Model is TValue)
            {
                var valueFromModel = (TValue)metadata.Model;
                return valueFromModel;
            }

            return default(TValue);
        }
    }
}