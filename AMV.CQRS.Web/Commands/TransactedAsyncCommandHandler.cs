using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;


namespace AMV.CQRS
{
    public class TransactedAsyncCommandHandler<TCommand> : IAsyncCommandHandler<TCommand> where TCommand : IAsyncCommand
    {
        public IAsyncCommandHandler<TCommand> Decorated { get; set; }
        private readonly ISuspendExecutionStrategy suspendExecutionStrategy;
        private readonly ILoggingService loggingService;

        public TransactedAsyncCommandHandler(
            IAsyncCommandHandler<TCommand> decorated,
            ISuspendExecutionStrategy suspendExecutionStrategy,
            ILoggingService loggingService)
        {
            Decorated = decorated;
            this.suspendExecutionStrategy = suspendExecutionStrategy;
            this.loggingService = loggingService;
        }


        public async Task HandleAsync(TCommand command)
        {
            if (command.GetType().GetCustomAttribute<TransactedCommandAttribute>() == null)
            {
                await Decorated.HandleAsync(command);
                return;
            }

            suspendExecutionStrategy.Suspend();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await Decorated.HandleAsync(command);
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