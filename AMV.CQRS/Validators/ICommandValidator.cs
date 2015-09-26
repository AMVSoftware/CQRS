namespace AMV.CQRS
{
    public interface ICommandValidator<in TCommand> where TCommand : ICommand 
    {
        ErrorList IsValid(TCommand command);
    }
}
