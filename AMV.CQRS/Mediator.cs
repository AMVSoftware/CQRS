using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;


namespace AMV.CQRS
{
    public class Mediator : IMediator
    {
        private readonly IServiceLocator container;

        public Mediator(IServiceLocator container)
        {
            this.container = container;
        }

        public virtual TResponseData Request<TResponseData>(IQuery<TResponseData> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponseData));
            var handler = container.GetInstance(handlerType);
            var result = (TResponseData)handler.GetType().GetMethod("Handle", new[] { query.GetType() }).Invoke(handler, new object[] { query });
            return result;
        }


        public async Task<TResult> RequestAsync<TResult>(IAsyncQuery<TResult> query)
        {
            var handlerType = typeof(IAsyncQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            var handler = container.GetInstance(handlerType);

            var result = await (Task<TResult>)handler.GetType().GetMethod("HandleAsync", new[] { query.GetType() }).Invoke(handler, new object[] { query });

            return result;
        }


        public async Task<ErrorList> ProcessCommandAsync<TCommand>(TCommand command) where TCommand : IAsyncCommand
        {
            var validator = container.GetInstance<IAsyncCommandValidator<TCommand>>();

            //Validator never null as we are always providing NullObject validator
            var validationResult = await validator.IsValidAsync(command);
            if (!validationResult.IsValid())
            {
                return validationResult;
            }

            var handler = container.GetInstance<IAsyncCommandHandler<TCommand>>();

            await handler.HandleAsync(command);

            return new ErrorList();
        }


        public ErrorList ProcessCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            var validator = container.GetInstance<ICommandValidator<TCommand>>();

            var errors = validator.IsValid(command);
            if (!errors.IsValid())
            {
                return errors;
            }

            var handler = container.GetInstance<ICommandHandler<TCommand>>();

            handler.Handle(command);

            return new ErrorList();
        }
    }
}