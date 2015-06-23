using System.Threading.Tasks;


namespace AMV.CQRS
{
    public interface IAsyncCommandHandler<in TCommand> where TCommand : IAsyncCommand
    {
        Task HandleAsync(TCommand command);
    }
}