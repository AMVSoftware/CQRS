using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Linq;
// ReSharper disable CheckNamespace


namespace AMV.CQRS
{
    //http://lostechies.com/jimmybogard/2013/11/07/null-collectionsarrays-from-mvc-model-binding/

    /// <summary>
    /// Creates non null collections on binding
    /// </summary>
    public class BetterDefaultModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// Trim strings
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="bindingContext"></param>
        /// <param name="propertyDescriptor"></param>
        /// <param name="value"></param>
        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
        {
            // if model property has [NoTrim] attribute, don't do trimming
            var noTrim = propertyDescriptor.Attributes.OfType<NoTrimAttribute>().ToList();
            if (propertyDescriptor.PropertyType == typeof(string) && !noTrim.Any())
            {
                var stringValue = (string)value;
                if (!string.IsNullOrEmpty(stringValue))
                {
                    stringValue = stringValue.Trim();
                }

                value = stringValue;
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var model = base.CreateModel(controllerContext, bindingContext, modelType);

            var isModelNull = model == null;
            var isModelEnumerable = model is IEnumerable;
            if (isModelNull || isModelEnumerable)
            {
                return model;
            }

            foreach (var property in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = property.GetValue(model);
                if (value != null)
                {
                    continue;
                }

                if (property.PropertyType.IsArray)
                {
                    value = Array.CreateInstance(property.PropertyType.GetElementType(), 0);
                    property.SetValue(model, value);
                }
                else if (property.PropertyType.IsGenericType)
                {
                    Type typeToCreate;
                    Type genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IDictionary<,>))
                    {
                        typeToCreate = typeof(Dictionary<,>).MakeGenericType(property.PropertyType.GetGenericArguments());
                    }
                    else if (genericTypeDefinition == typeof(IEnumerable<>) ||
                             genericTypeDefinition == typeof(ICollection<>) ||
                             genericTypeDefinition == typeof(List<>) ||
                             genericTypeDefinition == typeof(IList<>))
                    {
                        typeToCreate = typeof(List<>).MakeGenericType(property.PropertyType.GetGenericArguments());
                    }
                    else
                    {
                        continue;
                    }

                    value = Activator.CreateInstance(typeToCreate);
                    property.SetValue(model, value);
                }
            }

            return model;
        }
    }
}