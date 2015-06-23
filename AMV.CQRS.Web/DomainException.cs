using System;


namespace AMV.CQRS
{
    [Serializable]
    public class DomainException : Exception
    {
        public DomainException(string message)
            : base(message)
        {
        }

        public DomainException(string message, params object[] args)
            : this(string.Format(message, args))
        {
        }
    }
}
