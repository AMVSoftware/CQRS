//using System.Threading.Tasks;
//using Autofac;


//namespace AMV.CQRS
//{
//    public class Mediator : IMediator
//    {
//        readonly ILifetimeScope container;

//        public Mediator(ILifetimeScope container)
//        {
//            this.container = container;
//        }

//        public virtual TResponseData Request<TResponseData>(IQuery<TResponseData> query)
//        {
//            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponseData));
//            var handler = container.Resolve(handlerType);
//            var result = (TResponseData)handler.GetType().GetMethod("Handle", new[] { query.GetType() }).Invoke(handler, new object[] { query });
//            return result;
//        }


//        public async Task<TResult> RequestAsync<TResult>(IAsyncQuery<TResult> query)
//        {
//            var handlerType = typeof(IAsyncQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

//            var handler = container.Resolve(handlerType);

//            var result = await(Task<TResult>)handler.GetType().GetMethod("HandleAsync", new[] { query.GetType() }).Invoke(handler, new object[] { query });

//            return result;
//        }


//        public async Task<ErrorList> ProcessCommandAsync<TCommand>(TCommand command) where TCommand : IAsyncCommand
//        {
//            var validator = container.Resolve<IAsyncCommandValidator<TCommand>>();

//            //Validator never null as we are always providing NullObject validator
//            var validationResult = await validator.IsValidAsync(command);
//            if (!validationResult.IsValid())
//            {
//                return validationResult;
//            }

//            var handler = container.Resolve<IAsyncCommandHandler<TCommand>>();

//            await handler.HandleAsync(command);

//            return new ErrorList();
//        }


//        public ErrorList ProcessCommand<TCommand>(TCommand command) where TCommand : ICommand
//        {
//            var validator = container.Resolve<ICommandValidator<TCommand>>();

//            var isValid = validator.IsValid(command);
//            if (!isValid)
//            {
//                return validator.Errors;
//            }

//            var handler = container.Resolve<ICommandHandler<TCommand>>();

//            handler.Handle(command);

//            return new ErrorList();
//        }
//    }
//}