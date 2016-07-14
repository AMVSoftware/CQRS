using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace AMV.CQRS
{
    public class ProcessAsyncCommandBuilder<TCommand> where TCommand : IAsyncCommand
    {
        private readonly TCommand command;
        private readonly IMediator mediator;
        private readonly ModelStateDictionary modelState;
        private readonly HtmlHelper htmlHelper;

        private string successMessage;
        private ActionResult failure;
        private ActionResult success;

        public ProcessAsyncCommandBuilder(TCommand command, ModelStateDictionary modelState, IMediator mediator, HtmlHelper htmlHelper)
        {
            this.command = command;
            this.modelState = modelState;
            this.mediator = mediator;
            this.htmlHelper = htmlHelper;
        }

        public ProcessAsyncCommandBuilder<TCommand> SetRedirections(ActionResult failureAction, ActionResult successAction)
        {
            this.failure = failureAction;
            this.success = successAction;
            return this;
        }


        public ProcessAsyncCommandBuilder<TCommand> ShowMessage(String message)
        {
            successMessage = message;
            return this;
        }


        public virtual async Task<ActionResult> BuildAsync()
        {
            if (!modelState.IsValid)
            {
                return failure;
            }

            var errors = await mediator.ProcessCommandAsync(command);

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
