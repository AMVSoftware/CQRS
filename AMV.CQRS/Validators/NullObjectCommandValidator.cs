namespace AMV.CQRS
{
    public class NullObjectCommandValidator<TCommand>
        : ICommandValidator<TCommand> where TCommand : ICommand 
    {
        public ErrorList IsValid(TCommand command)
        {
            return new ErrorList();
        }
    }
}
