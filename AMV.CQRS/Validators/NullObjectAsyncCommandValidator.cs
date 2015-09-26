using System.Threading.Tasks;


namespace AMV.CQRS
{
    public class NullObjectAsyncCommandValidator<TCommand>
        : IAsyncCommandValidator<TCommand> where TCommand : IAsyncCommand
    {
        public Task<ErrorList> IsValidAsync(TCommand command)
        {
            return Task.FromResult(new ErrorList());
        }
    }
}