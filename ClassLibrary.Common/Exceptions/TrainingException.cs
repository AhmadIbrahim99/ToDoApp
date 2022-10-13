using System;

namespace ClassLibrary.Common.Exceptions
{
    public class TrainingException : Exception
    {
        public int ErrorCode { get; set; }
        public TrainingException(): base("TrainingException")
        {

        }
        public TrainingException(string msg) : base(msg)
        {

        }
        public TrainingException(string msg, Exception innerException) : base(msg, innerException)
        {

        }

        public TrainingException(int StatusCode,string msg) : base(msg)
        {
            ErrorCode = StatusCode;
        }
        public TrainingException(int StatusCode, string msg, Exception innerException) : base(msg, innerException)
        {
            ErrorCode = StatusCode;
        }
    }
}
