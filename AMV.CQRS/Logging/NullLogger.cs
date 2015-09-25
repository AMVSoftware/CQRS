using System;


namespace AMV.CQRS
{
    public class NullLogger : ILoggingService
    {
        public void SetLoggerName(string name)
        {
        }

        public void Info(string message, params object[] args)
        {
        }


        public void Error(string message, params object[] args)
        {
        }


        public void ErrorException(string message, Exception exception)
        {
        }


        public void Debug(Exception exception)
        {
        }


        public void Debug(string message, Exception exception)
        {
        }


        public void Debug(string message, params object[] args)
        {
        }


        public void Trace(string message, params object[] args)
        {
        }


        public void Warn(string message, params object[] args)
        {
        }
    }
}
