using System;


namespace AMV.CQRS
{
    public class TraceLogger : ILoggingService
    {
        private String loggerName = String.Empty;

        public void SetLoggerName(string name)
        {
            loggerName = String.Format("[{0}]", name);
        }


        public void Info(string message, params object[] args)
        {
            WriteFormattedMessage("Info", message, args);
        }
        public void Info(string message, Exception exception)
        {
            WriteException("Info", message, exception);
        }


        public void Error(string message, params object[] args)
        {
            WriteFormattedMessage("Error", message, args);
        }


        public void ErrorException(string message, Exception exception)
        {
            WriteException("Error", message, exception);
        }


        public void Debug(Exception exception)
        {
            WriteException("Debug", exception.Message, exception);
        }

        public void Debug(string message, params object[] args)
        {
            WriteFormattedMessage("Debug", message, args);
        }
        public void Debug(string message, Exception exception)
        {
            WriteException("Debug", message, exception);
        }


        public void Trace(string message, params object[] args)
        {
            WriteFormattedMessage("Trace", message, args);
        }


        public void Warn(string message, params object[] args)
        {
            WriteFormattedMessage("Warn", message, args);
        }


        private void WriteException(String level, String message, Exception exception)
        {
            var finalMessage = String.Format("{0} {1}: {2}; Exception: {3}", loggerName, level.ToUpper(), message, exception);

            System.Diagnostics.Trace.WriteLine(finalMessage);
        }

        private void WriteFormattedMessage(string level, string message, object[] args)
        {
            var template = String.Format("{0} {1}: {2}", loggerName, level.ToUpper(), message);
            var finalMessage = String.Format(template, args);
            System.Diagnostics.Trace.WriteLine(finalMessage);
        }
    }
}
