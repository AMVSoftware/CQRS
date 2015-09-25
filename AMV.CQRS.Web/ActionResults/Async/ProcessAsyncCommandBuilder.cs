using System;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace AMV.CQRS
{
    public class ProcessAsyncCommandBuilder<TCommand> where TCommand : IAsyncCommand
    {
        private readonly TCommand command;
        private readonly IMediator mediator;
        private readonly ModelStateDictionary modelState;
        private readonly TempDataDictionary tempData;

        private string successMessage;
        private ActionResult failure;
        private ActionResult success;

        public ProcessAsyncCommandBuilder(TCommand command, ModelStateDictionary modelState, TempDataDictionary tempData, IMediator mediator)
        {
            this.command = command;
            this.modelState = modelState;
            this.tempData = tempData;
            this.mediator = mediator;
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
                tempData["SuccessMessage"] = successMessage;
            }

            return success;
        }
    }
}
