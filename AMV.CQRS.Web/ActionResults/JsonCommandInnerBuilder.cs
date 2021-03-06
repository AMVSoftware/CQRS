﻿using System;
using System.Web;
using System.Web.Mvc;


namespace AMV.CQRS
{
    internal class JsonCommandInnerBuilder<TCommand>
    {
        public TCommand Command { get; private set; }
        public ModelStateDictionary ModelState { get; private set; }
        public HtmlHelper HtmlHelper { get; private set; }
        public IMediator Mediator { get; private set; }
        public ILoggingService LoggingService { get; private set; }

        public ActionResult RedirectTo { get; set; }
        public JsonResult JsonPayloadResult { get; set; }
        public String RedirectUrl { get; set; }
        public string SuccessMessage { get; set; }
        public bool ReloadPage { get; set; }


        public JsonCommandInnerBuilder(TCommand command, ModelStateDictionary modelState, HtmlHelper htmlHelper, IMediator mediator, ILoggingService loggingService)
        {
            this.Command = command;
            this.ModelState = modelState;
            this.HtmlHelper = htmlHelper;
            this.Mediator = mediator;
            this.LoggingService = loggingService;
        }


        public ActionResult HandleException(Exception exception)
        {
            ExceptionHandler.Current.Invoke(exception);
            LoggingService.ErrorException("Failed to process command", exception);
            if (exception is DomainException)
            {
                ModelState.AddModelError("", exception.Message);
            }
            else
            {
#if (DEBUG)
                ModelState.AddModelError("", "Unable to process command " + exception);
#else
                ModelState.AddModelError("", "Unable to process command");
#endif
            }
            return CommandBuilderHelpers.ReturnJsonModelState(ModelState);
        }


        public ActionResult Handle()
        {
            if (JsonPayloadResult != null)
            {
                return JsonPayloadResult;
            }

            if (ReloadPage)
            {
                AddSuccessMessage(SuccessMessage);
                var reloadResult = new CustomJsonResult
                {
                    Data = new
                    {
                        IsPageReload = true,
                    }
                };

                return reloadResult;
            }

            var generatedRedirectUrl = CommandBuilderHelpers.GetRedirectUrl(RedirectUrl, RedirectTo, HtmlHelper);
            if (generatedRedirectUrl != null)
            {
                AddSuccessMessage(SuccessMessage);

                var redirectResult = new CustomJsonResult
                {
                    Data = new
                    {
                        RedirectUrl = generatedRedirectUrl,
                        IsRedirect = true
                    }
                };

                return redirectResult;
            }


            if (!String.IsNullOrEmpty(SuccessMessage))
            {
                var jsonResult = new CustomJsonResult
                {
                    Data = new
                    {
                        SuccessMessage = SuccessMessage,
                        Success = true
                    }
                };

                return jsonResult;
            }

            return new HttpStatusCodeResult(201);
        }


        private void AddSuccessMessage(string successMessage)
        {
            var httpContextBase = HtmlHelper.ViewContext.RequestContext.HttpContext;
            httpContextBase.Response.Cookies.Add(new HttpCookie("SuccessMessage", successMessage) { Path = "/", HttpOnly = false });
        }
    }
}