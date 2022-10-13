using Serilog.Events;
using System;

namespace WebApplication5.Exceptions.Logging
{
    public class LogMessage
    {
        public int UserId { get; set; } = 0;
        public string Message { get; set; } = "";
        public DateTime  CreatedOn { get; set; } = DateTime.Now;
        public string UserEmail { get; set; } = "";
        public string ApplicationName { get; set; } = "";
        public string RequestPath { get; set; } = "";
        public string ExceptionName { get; set; } = "";
        public LogEventLevel LogLevel { get; set; }

    }
}
