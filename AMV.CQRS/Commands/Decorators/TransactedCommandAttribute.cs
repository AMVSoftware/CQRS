using System;


namespace AMV.CQRS
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TransactedCommandAttribute : Attribute
    {
        // marker attribute for commands to be wrapped into a transaction
        // to be applied on ICommand objects
    }
}