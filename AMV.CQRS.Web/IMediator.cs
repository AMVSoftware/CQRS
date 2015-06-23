using System.Threading.Tasks;


namespace AMV.CQRS
{
    public interface IMediator
    {
        TResponse Request<TResponse>(IQuery<TResponse> query);

        Task<ErrorList> ProcessCommandAsync<TCommand>(TCommand command) where TCommand : IAsyncCommand;
        Task<TResult> RequestAsync<TResult>(IAsyncQuery<TResult> query);
        ErrorList ProcessCommand<TCommand>(TCommand command) where TCommand : ICommand;
    }
}