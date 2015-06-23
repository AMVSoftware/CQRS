using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AMV.Helpers;

namespace AMV.CQRS
{
    /// <summary>
    /// Cleverness from Jimmy Bogard. 
    /// Whenever a model property does not have Display Attribute, we add it here: separate words with spaces.
    /// </summary>
    public class ConventionProvider : DataAnnotationsModelMetadataProvider
    {
        private static readonly Regex IdRegex = new Regex(@"(.*)(Id[s]?)", RegexOptions.IgnoreCase & RegexOptions.Compiled);

        protected override ModelMetadata CreateMetadata(
            IEnumerable<Attribute> attributes,
            Type containerType,
            Func<object> modelAccessor,
            Type modelType,
            string propertyName)
        {
            var meta = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            if (meta.DisplayName != null || meta.PropertyName == null)
            {
                return meta;
            }
            meta.DisplayName = meta.PropertyName;
                
            if (meta.DisplayName.Length > 2 && (meta.ModelType.FullName.Contains("Int")|| (meta.ModelType.FullName.Contains("Guid"))))
            {
                var match = IdRegex.Match(meta.DisplayName);
                if (match.Success)
                {
                    meta.DisplayName = match.Groups[1].Value;    
                }
            }
            meta.DisplayName = meta.DisplayName.ToSeparatedWords().LowerCasePrepositions();
            return meta;
        }
    }
}