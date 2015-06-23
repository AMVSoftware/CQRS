using System;
using System.Reflection;
using System.Transactions;


namespace AMV.CQRS
{
    public class TransactedCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        public ICommandHandler<TCommand> Decorated { get; set; }
        private readonly ISuspendExecutionStrategy suspendExecutionStrategy;
        private readonly ILoggingService loggingService;

        public TransactedCommandHandler(
            ICommandHandler<TCommand> decorated, 
            ISuspendExecutionStrategy suspendExecutionStrategy, 
            ILoggingService loggingService)
        {
            Decorated = decorated;
            this.suspendExecutionStrategy = suspendExecutionStrategy;
            this.loggingService = loggingService;
        }


        public void Handle(TCommand command)
        {
            if (command.GetType().GetCustomAttribute<TransactedCommandAttribute>() == null)
            {
                Decorated.Handle(command);
                return;
            }

            suspendExecutionStrategy.Suspend();

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Decorated.Handle(command);
                    scope.Complete();
                }
                catch (Exception exception)
                {
                    scope.Dispose();
                    loggingService.ErrorException("Failed to complete transaction", exception);
                    throw;
                }
                finally
                {
                    suspendExecutionStrategy.Unsuspend();
                }
            }
        }
    }
}
