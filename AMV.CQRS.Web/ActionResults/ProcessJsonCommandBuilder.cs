using System;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace AMV.CQRS
{
    public class ProcessJsonCommandBuilder<TCommand> where TCommand : ICommand
    {
        private readonly JsonCommandInnerBuilder<TCommand> innerBuilder;

        public ProcessJsonCommandBuilder(TCommand command,
                                         ModelStateDictionary modelState,
                                         HtmlHelper htmlHelper,
                                         IMediator mediator,
                                         ILoggingService loggingService)
        {
            innerBuilder = new JsonCommandInnerBuilder<TCommand>(command, modelState, htmlHelper, mediator, loggingService);
        }


        public static implicit operator ActionResult(ProcessJsonCommandBuilder<TCommand> processorBuilder)
        {
            return processorBuilder.Build();
        }


        public ProcessJsonCommandBuilder<TCommand> ShowMessage(String message)
        {
            innerBuilder.SuccessMessage = message;
            return this;
        }


        public ProcessJsonCommandBuilder<TCommand> RedirectTo(ActionResult redirection)
        {
            innerBuilder.RedirectTo = redirection;
            return this;
        }


        public ProcessJsonCommandBuilder<TCommand> RedirectTo(string url)
        {
            innerBuilder.RedirectUrl = url;
            return this;
        }


        public ProcessJsonCommandBuilder<TCommand> ReloadPage()
        {
            innerBuilder.ReloadPage = true;
            return this;
        }


        public ProcessJsonCommandBuilder<TCommand> RedirectTo(Task<ActionResult> redirection)
        {
            innerBuilder.RedirectTo = redirection.Result;
            return this;
        }


        public ProcessJsonCommandBuilder<TCommand> ReturnJson(JsonResult jsonResult)
        {
            innerBuilder.JsonPayloadResult = jsonResult;
            return this;
        }


        public virtual ActionResult Build()
        {
            // check if model was submitted in a good state
            if (!innerBuilder.ModelState.IsValid)
            {
                return CommandBuilderHelpers.ReturnJsonModelState(innerBuilder.ModelState);
            }

            try
            {
                var errors = innerBuilder.Mediator.ProcessCommand(innerBuilder.Command);
                if (!errors.IsSuccess())
                {
                    innerBuilder.ModelState.AddErrorMessages(errors);

                    return CommandBuilderHelpers.ReturnJsonModelState(innerBuilder.ModelState);
                }
            }
            catch (Exception exception)
            {
                innerBuilder.HandleException(exception);
            }

            return innerBuilder.Handle();
        }
    }
}