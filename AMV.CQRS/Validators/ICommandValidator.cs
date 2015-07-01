namespace AMV.CQRS
{
    public interface ICommandValidator<in TCommand> where TCommand : ICommand 
    {
        /// <summary>
        /// Errors are coming in sets of two: error message and fieldName that is applicable to.
        /// First Key is usually a field name on the form, where value is an error message to be displayed.
        /// </summary>
        ErrorList Errors { get; }

        bool IsValid(TCommand command);
    }
}
