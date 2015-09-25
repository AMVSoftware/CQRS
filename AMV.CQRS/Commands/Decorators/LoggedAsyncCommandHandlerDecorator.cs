using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace AMV.CQRS
{
    public class LoggedAsyncCommandHandlerDecorator<TCommand> : IAsyncCommandHandler<TCommand> where TCommand : IAsyncCommand
    {
        public IAsyncCommandHandler<TCommand> Decorated { get; set; }
        private readonly ILoggingService logger;
        private readonly IContractResolver contractResolver;

        public LoggedAsyncCommandHandlerDecorator(
            IAsyncCommandHandler<TCommand> decorated, 
            ILoggingService logger, 
            IContractResolver contractResolver)
        {
            Decorated = decorated;
            this.contractResolver = contractResolver;
            this.logger = logger;
            logger.SetLoggerName("Command Handler");
        }


        public async Task HandleAsync(TCommand command)
        {
            String serialisedData;
            try
            {
                serialisedData = JsonConvert.SerializeObject(command, Formatting.None, new JsonSerializerSettings
                                                                                           {
                                                                                               ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                                                                               ContractResolver = contractResolver,
                                                                                           });
            }
            catch (Exception)
            {
                serialisedData = "Unable to serialize command";
            }

            logger.Info("About to handle command handler of type {0} with data {1}", command.GetType().Name, serialisedData);

            await Decorated.HandleAsync(command);

            logger.Info("Finished with command handler of type {0}", command.GetType().Name);
        }
    }
}