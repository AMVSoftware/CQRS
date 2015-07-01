using System.Threading.Tasks;


namespace AMV.CQRS
{
    public class NullObjectAsyncCommandValidator<TCommand>
        : IAsyncCommandValidator<TCommand> where TCommand : IAsyncCommand
    {
        public ErrorList Errors { get; private set; }

        public NullObjectAsyncCommandValidator()
        {
            Errors = new ErrorList();
        }

        public Task<ErrorList> IsValidAsync(TCommand command)
        {
            return Task.FromResult(new ErrorList());
        }
    }
}