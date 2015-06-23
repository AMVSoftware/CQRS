using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMV.CQRS.Web.ActionResults;


namespace AMV.CQRS
{
    public class ProcessJsonCommandBuilder<TCommand> where TCommand : ICommand
    {
        private readonly TCommand command;
        private readonly HtmlHelper htmlHelper;
        private readonly IMediator mediator;
        private readonly ILoggingService loggingService;
        private readonly ModelStateDictionary modelState;
        private readonly TempDataDictionary tempData;

        private ActionResult redirectTo;
        private JsonResult jsonPayloadResult;
        private String redirectUrl;
        private string successMessage;
        private bool reloadPage;

        public ProcessJsonCommandBuilder(TCommand command, ModelStateDictionary modelState, HtmlHelper htmlHelper, TempDataDictionary tempData, IMediator mediator, ILoggingService loggingService)
        {
            this.command = command;
            this.modelState = modelState;
            this.htmlHelper = htmlHelper;
            this.tempData = tempData;
            this.mediator = mediator;
            this.loggingService = loggingService;
            this.loggingService.SetLoggerName("ProcessJsonCommandBuilder");
        }


        public static implicit operator ActionResult(ProcessJsonCommandBuilder<TCommand> processorBuilder)
        {
            return processorBuilder.Build();
        }


        public ProcessJsonCommandBuilder<TCommand> ShowMessage(String message)
        {
            successMessage = message;
            return this;
        }


        public ProcessJsonCommandBuilder<TCommand> RedirectTo(ActionResult redirection)
        {
            redirectTo = redirection;
            return this;
        }

        public ProcessJsonCommandBuilder<TCommand> RedirectTo(string url)
        {
            redirectUrl = url;
            return this;
        }


        public ProcessJsonCommandBuilder<TCommand> ReloadPage()
        {
            this.reloadPage = true;
            return this;
        }


        public ProcessJsonCommandBuilder<TCommand> RedirectTo(Task<ActionResult> redirection)
        {
            redirectTo = redirection.Result;
            return this;
        }


        public ProcessJsonCommandBuilder<TCommand> ReturnJson(JsonResult jsonResult)
        {
            jsonPayloadResult = jsonResult;
            return this;
        }


        public virtual ActionResult Build()
        {
            // check if model was submitted in a good state
            if (!modelState.IsValid)
            {
                return ReturnJsonModelState();
            }

            try
            {
                var errors = mediator.ProcessCommand(command);
                if (!errors.IsSuccess())
                {
                    modelState.AddErrorMessages(errors);

                    return ReturnJsonModelState();
                }
            }
            catch (DomainException domainException)
            {
                ExceptionHandler.Current.Invoke(domainException);
                loggingService.ErrorException("Failed to process command", domainException);
                modelState.AddModelError("", domainException.Message);
                return ReturnJsonModelState();
            }
            catch (Exception exception)
            {
                ExceptionHandler.Current.Invoke(exception);
                loggingService.ErrorException("Failed to process command", exception);
#if (DEBUG)
                modelState.AddModelError("", "Unable to process command " + exception);
#else
                modelState.AddModelError("", "Unable to process command");
#endif
                return ReturnJsonModelState();
            }

            if (jsonPayloadResult != null)
            {
                return jsonPayloadResult;
            }

            if (reloadPage)
            {
                tempData.Add("SuccessMessage", successMessage);
                var reloadResult = new CustomJsonResult
                {
                    Data = new
                    {
                        IsPageReload = true,
                    }
                };

                return reloadResult;
            }

            if (GetRedirectUrl() != null)
            {
                // set TempData to have the message
                tempData.Add("SuccessMessage", successMessage);

                var redirectResult = new CustomJsonResult
                {
                    Data = new
                    {
                        RedirectUrl = GetRedirectUrl(),
                        IsRedirect = true
                    }
                };

                return redirectResult;
            }


            if (!String.IsNullOrEmpty(successMessage))
            {
                var jsonResult = new CustomJsonResult
                {
                    Data = new
                    {
                        SuccessMessage = successMessage,
                        Success = true
                    }
                };

                return jsonResult;
            }


            return new HttpStatusCodeResult(201);
        }


        private String GetRedirectUrl()
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


        private ActionResult ReturnJsonModelState()
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
