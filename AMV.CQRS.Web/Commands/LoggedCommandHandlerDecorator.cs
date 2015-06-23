using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace AMV.CQRS
{
    public class LoggedCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        public ICommandHandler<TCommand> Decorated { get; set; }
        private readonly ILoggingService logger;
        private readonly IContractResolver contractResolver;

        public LoggedCommandHandlerDecorator(ICommandHandler<TCommand> decorated, ILoggingService logger, IContractResolver contractResolver)
        {
            Decorated = decorated;
            this.logger = logger;
            this.contractResolver = contractResolver;
            logger.SetLoggerName("Command Handler");
        }


        public void Handle(TCommand command)
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

            Decorated.Handle(command);

            logger.Info("Finished with command handler of type {0}", command.GetType().Name);
        }
    }
}
