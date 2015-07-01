using System;


namespace AMV.CQRS
{
    public class ErrorMessage
    {
        public string FieldName { get; set; }
        public string Message { get; set; }

        public ErrorMessage(String message)
        {
            FieldName = string.Empty;
            Message = message;
        }

        public ErrorMessage(String fieldName, String message)
        {
            FieldName = fieldName;
            Message = message;
        }

        public ErrorMessage(string fieldName, string message, params object[] args)
            : this(fieldName, string.Format(message, args))
        {
        }

        public override bool Equals(object obj)
        {
            var compare = obj as ErrorMessage;
            return FieldName == compare.FieldName && Message == compare.Message;
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + FieldName.GetHashCode();
            hash = (hash * 7) + Message.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            if (String.IsNullOrEmpty(FieldName))
            {
                return Message;
            }
            return FieldName + ": " + Message;
        }
    }
}
