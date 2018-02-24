using System;
using Microsoft.Extensions.Logging;

namespace LoggerRecord
{
    public class LoggerExtensions
    {
        private static readonly Action<ILogger,string,Exception>_loggerRecord;
        static LoggerExtensions()
        {
            _loggerRecord=LoggerMessage.Define<string>(LogLevel.Information,new EventId(1,nameof(LoggerRecord)),
            "Record logging");
        }

        public static void LoggerRecord(ILogger logger,string message)
        {
            _loggerRecord(logger,message,null);
        }
    }
}
