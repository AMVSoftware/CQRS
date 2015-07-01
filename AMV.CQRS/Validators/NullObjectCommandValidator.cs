namespace AMV.CQRS
{
    public class NullObjectCommandValidator<TCommand>
        : ICommandValidator<TCommand> where TCommand : ICommand 
    {
        public ErrorList Errors { get; private set; }

        public NullObjectCommandValidator()
        {
            Errors = new ErrorList();
        }

        public bool IsValid(TCommand command)
        {
            return true;
        }
    }
}
