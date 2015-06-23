using System.Threading.Tasks;


namespace AMV.CQRS
{
    public interface IAsyncCommandValidator<in TCommand> where TCommand : IAsyncCommand
    {
        Task<ErrorList> IsValidAsync(TCommand command);
    }
}