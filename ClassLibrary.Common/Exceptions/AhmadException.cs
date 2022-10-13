namespace WebApplication5.Exceptions
{
    public class AhmadException : TrainingException
    {
        public AhmadException() : base("Service Validation Exception")
        {

        }

        public AhmadException(string msg) : base(msg)
        {

        }

        public AhmadException(int StatusCode, string msg) : base(StatusCode,msg)
        {
        }
    }
}
