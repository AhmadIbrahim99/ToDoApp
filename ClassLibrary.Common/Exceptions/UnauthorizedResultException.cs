using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication5.Exceptions;

namespace ClassLibrary.Common.Exceptions
{
    public class UnauthorizedResultException : TrainingException
    {
        public UnauthorizedResultException() : base("UnauthorizedResultException")
        {

        }
        public UnauthorizedResultException(string msg) : base(msg)
        {

        }
        public UnauthorizedResultException(string msg, Exception innerException) : base(msg, innerException)
        {

        }

        public UnauthorizedResultException(int StatusCode, string msg) : base(msg)
        {
            ErrorCode = StatusCode;
        }
        public UnauthorizedResultException(int StatusCode, string msg, Exception innerException) : base(msg, innerException)
        {
            
            ErrorCode = StatusCode;
        }
    }
}
