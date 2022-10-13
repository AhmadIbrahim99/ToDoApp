//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading.Tasks;
//using System.Web.Http.ExceptionHandling;

//namespace WebApplication5.Exceptions
//{
//    public class MyExceptionHandler : ExceptionHandler
//    {
//        public RequestDelegate _next { get; set; }
//        private readonly ILogger _logger;
//        public MyExceptionHandler(RequestDelegate next, ILoggerFactory loggerFactory) 
//        {
//            _logger = loggerFactory.CreateLogger<MyExceptionHandler>();
//            _next = next;
//        }
//        public async Task Invoke(HttpContext context)
//        {
//            try
//            {
//                await _next(context);
//            }
//            catch (Exception e)
//            {

//                HandleException(e);
//            }
//        }
//        public void HandleException(Exception e)
//        {
//            if (e == null) return;
//            if (e is ArgumentException)
//                _logger.LogError(e.Message);
//            else if (e is InvalidOperationException)
//                _logger.LogInformation(e.Message);
//            else if (e is AhmadException ahmadException)
//                _logger.LogInformation(ahmadException.Message + ahmadException.ErrorCode);
//            else
//                _logger.LogError(0, e.Message, "test");


//        }
//    }
//}
