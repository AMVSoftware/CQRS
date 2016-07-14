using System;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace AMV.CQRS
{
    public class ProcessJsonAsyncCommandBuilder<TCommand> where TCommand : IAsyncCommand
    {
        private readonly JsonCommandInnerBuilder<TCommand> innerBuilder;

        public ProcessJsonAsyncCommandBuilder(TCommand command, 
                                              ModelStateDictionary modelState, 
                                              HtmlHelper htmlHelper, 
                                              IMediator mediator, 
                                              ILoggingService loggingService)
        {
            innerBuilder = new JsonCommandInnerBuilder<TCommand>(command, modelState, htmlHelper, mediator, loggingService);
        }


        public static implicit operator Task<ActionResult>(ProcessJsonAsyncCommandBuilder<TCommand> processorBuilder)
        {
            return processorBuilder.Build();
        }


        public ProcessJsonAsyncCommandBuilder<TCommand> ShowMessage(String message)
        {
            innerBuilder.SuccessMessage = message;
            return this;
        }


        public ProcessJsonAsyncCommandBuilder<TCommand> RedirectTo(ActionResult redirection)
        {
            innerBuilder.RedirectTo = redirection;
            return this;
        }

        public ProcessJsonAsyncCommandBuilder<TCommand> RedirectTo(string url)
        {
            innerBuilder.RedirectUrl = url;
            return this;
        }


        public ProcessJsonAsyncCommandBuilder<TCommand> ReloadPage()
        {
            innerBuilder.ReloadPage = true;
            return this;
        }


        public ProcessJsonAsyncCommandBuilder<TCommand> RedirectTo(Task<ActionResult> redirection)
        {
            innerBuilder.RedirectTo = redirection.Result;
            return this;
        }


        public ProcessJsonAsyncCommandBuilder<TCommand> ReturnJson(JsonResult jsonResult)
        {
            innerBuilder.JsonPayloadResult = jsonResult;
            return this;
        }


        public virtual async Task<ActionResult> Build()
        {
            // check if model was submitted in a good state
            if (!innerBuilder.ModelState.IsValid)
            {
                return CommandBuilderHelpers.ReturnJsonModelState(innerBuilder.ModelState);
            }

            try
            {
                var errors = await innerBuilder.Mediator.ProcessCommandAsync(innerBuilder.Command);
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
