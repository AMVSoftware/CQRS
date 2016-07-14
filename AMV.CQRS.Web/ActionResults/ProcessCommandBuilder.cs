using System;
using System.Web;
using System.Web.Mvc;


namespace AMV.CQRS
{
    public class ProcessCommandBuilder<TCommand> where TCommand : ICommand
    {
        private readonly TCommand command;
        private readonly IMediator mediator;
        private readonly ModelStateDictionary modelState;
        private readonly HtmlHelper htmlHelper;

        private string successMessage;
        private ActionResult failure;
        private ActionResult success;

        public ProcessCommandBuilder(TCommand command, ModelStateDictionary modelState, IMediator mediator, HtmlHelper htmlHelper)
        {
            this.command = command;
            this.modelState = modelState;
            this.mediator = mediator;
            this.htmlHelper = htmlHelper;
        }


        public static implicit operator ActionResult(ProcessCommandBuilder<TCommand> processorBuilder)
        {
            return processorBuilder.Build();
        }


        public ProcessCommandBuilder<TCommand> SetRedirections(ActionResult failureAction, ActionResult successAction)
        {
            this.failure = failureAction;
            this.success = successAction;
            return this;
        }


        public ProcessCommandBuilder<TCommand> ShowMessage(String message)
        {
            successMessage = message;
            return this;
        }


        public virtual ActionResult Build()
        {
            if (!modelState.IsValid)
            {
                return failure;
            }

            var errors = mediator.ProcessCommand(command);

            modelState.AddErrorMessages(errors);

            if (!modelState.IsValid)
            {
                return failure;
            }

            if (!String.IsNullOrEmpty(successMessage))
            {
                AddSuccessMessage(successMessage);
            }

            return success;
        }

        private void AddSuccessMessage(string message)
        {
            var httpContextBase = htmlHelper.ViewContext.RequestContext.HttpContext;
            httpContextBase.Response.Cookies.Add(new HttpCookie("SuccessMessage", message) { Path = "/", HttpOnly = false });
        }

    }
}
