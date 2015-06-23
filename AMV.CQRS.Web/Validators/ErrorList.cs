using System;
using System.Collections.Generic;
using System.Linq;


namespace AMV.CQRS
{
    /// <summary>
    /// ErrorList simply inherits List<IErrorMessage>
    /// It allows us to override ToString() so that we can debug error messages easily in tests 
    /// It provide a ToString implemenation for Error message and it will be included in ToString
    /// when called on ErrorList
    /// </summary>
    public class ErrorList : List<ErrorMessage>
    {
        public override string ToString()
        {
            return this.Aggregate("", (current, next) => current + ", " + next);
        }


        public void AddError(String message)
        {
            this.Add(message);
        }


        public void Add(String message)
        {
            this.Add(new ErrorMessage(message));
        }


        public void Add(string fieldName, string message)
        {
            this.Add(new ErrorMessage(fieldName, message));
        }


        public void Add(String fieldName, String message, params object[] args)
        {
            this.Add(new ErrorMessage(fieldName, message, args));
        }


        public bool IsValid()
        {
            return !this.Any();
        }


        public bool IsSuccess()
        {
            return IsValid();
        }


        public String ToCsv()
        {
            return String.Join(", ", this.Select(e => e.ToString()));
        }


        public ErrorList Merge(ErrorList otherErrors)
        {
            this.AddRange(otherErrors);

            return this;
        }
    }
}
