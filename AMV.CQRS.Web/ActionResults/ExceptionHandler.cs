using System;


// ReSharper disable FieldCanBeMadeReadOnly.Global
namespace AMV.CQRS
{
    public static class ExceptionHandler
    {
        /// <summary>
        /// Action to process exceptoins raised by command handlers.
        /// Targeted to decouple Elmah from this library.
        /// Assign this action in the startup code: 
        /// AMV.CQRS.ExceptionHandler.Current = exception =>
        ///     {
        ///         Elmah.ErrorSignal.FromCurrentContext().Raise(exception);
        ///     };
        /// 
        /// By default this is set to rethrow the exception.
        /// </summary>
        public static Action<Exception> Current = exception => { throw exception; };
    }
}
