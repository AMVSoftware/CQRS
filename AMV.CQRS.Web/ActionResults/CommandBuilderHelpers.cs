using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace AMV.CQRS
{
    internal static class CommandBuilderHelpers
    {
        public static String GetRedirectUrl(String redirectUrl, ActionResult redirectTo, HtmlHelper htmlHelper)
        {
            if (!String.IsNullOrEmpty(redirectUrl))
            {
                return redirectUrl;
            }

            if (redirectTo != null)
            {
                return redirectTo.GenerateUrl(htmlHelper);
            }

            return null;
        }


        public static ActionResult ReturnJsonModelState(ModelStateDictionary modelState)
        {
            var genericErrors = new List<String>();
            var fieldErrors = new Dictionary<String, String>();

            foreach (var fieldState in modelState.Where(f => f.Value.Errors.Any()))
            {
                var fieldName = fieldState.Key;
                var errors = fieldState.Value.Errors;
                if (!String.IsNullOrEmpty(fieldName))
                {
                    foreach (var e in errors.ToList())
                    {
                        if (fieldErrors.ContainsKey(fieldName))
                        {
                            fieldErrors[fieldName] = fieldErrors[fieldName] + ". " + e.ErrorMessage;
                        }
                        else
                        {
                            fieldErrors.Add(fieldName, e.ErrorMessage);
                        }
                    }
                }
                else
                {
                    errors.ToList().ForEach(e => genericErrors.Add(e.ErrorMessage));
                }
            }

            var jsonResult = new CustomJsonResult()
            {
                Data = new
                {
                    validationFail = true,
                    fieldErrors = fieldErrors,
                    genericErrors = genericErrors,
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };

            return jsonResult;
        }
    }
}
