using System.Web.Mvc;


namespace AMV.CQRS
{
    public class CommandProcessorBuilderFactory<TCommand> where TCommand : ICommand
    {
        private readonly TCommand command;
        private readonly HtmlHelper htmlHelper;
        private readonly IMediator mediator;
        private readonly ILoggingService loggingService;
        private readonly ModelStateDictionary modelState;
        private readonly TempDataDictionary tempData;


        public CommandProcessorBuilderFactory(TCommand command, HtmlHelper htmlHelper, ModelStateDictionary modelState, TempDataDictionary tempData)
        {
            this.command = command;
            this.htmlHelper = htmlHelper;
            this.modelState = modelState;
            this.tempData = tempData;
            mediator = DependencyResolver.Current.GetService<IMediator>();
            loggingService = DependencyResolver.Current.GetService<ILoggingService>();
        }


        public ProcessCommandBuilder<TCommand> DoRedirects(ActionResult failure, ActionResult success)
        {
            var processCommandBuilder = new ProcessCommandBuilder<TCommand>(command, modelState, tempData, mediator).SetRedirections(failure, success);

            return processCommandBuilder;
        }


        public ProcessJsonCommandBuilder<TCommand> DoJsonValidation()
        {
            var jsonCommandBuilder = new ProcessJsonCommandBuilder<TCommand>(command, modelState, htmlHelper, tempData, mediator, loggingService);

            return jsonCommandBuilder;
        }
    }
}
