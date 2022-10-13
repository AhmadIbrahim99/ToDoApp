using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApplication5.Exceptions;
using WebApplication5.Exceptions.Logging;

namespace WebApplication5.Extenstions
{
    public static class ExceptionHandleMiddleWareExtention
    {
        private static readonly HashSet<string> HandledExceptions = new HashSet<string>(){ 
        typeof(TrainingException).FullName,
        typeof(AhmadException).FullName,
        typeof(UnauthorizedResultException).FullName
        };
        
        public static void ConfigureExceptionHandler(this IApplicationBuilder app,
            ILogger logger
            , IWebHostEnvironment env
            ,string ApplicationName) 
        {
            app.UseExceptionHandler(appError => 
            {
                appError.Run(
                    async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                        if (contextFeature == null) return;
                        #region HandleException
                        try
                        {
                            await LogExceptionAsync(logger, context, contextFeature.Error, ApplicationName).AnyContext();
                        }
                        catch (Exception)
                        { }
                        var exception = contextFeature.Error;
                        var errorCode = 0;
                        var responseText = env.IsDevelopment() || env.IsEnvironment("Local")
                        ? exception?.Message : "Something went wrong";
                        if (HandledExceptions.Contains(exception.GetType().FullName))
                        {
                            int statusCode = (int)HttpStatusCode.BadRequest;
                            switch (exception.GetBaseException())
                            {
                                case TrainingException ex:
                                    statusCode = ex.ErrorCode == 406 ? 
                                    (int)HttpStatusCode.OK : (int)HttpStatusCode
                                    .BadRequest;
                                    errorCode = ex.ErrorCode;
                                    break;
                                default:
                                    break;
                            }
                            #region CodeIfStatement Old
                            /*
                           if (exception is TrainingException ex)
                         {
                             statusCode = ex.ErrorCode == 406 ? (int)HttpStatusCode.OK : 
                            (int)HttpStatusCode.BadRequest;
                             errorCode = ex.ErrorCode;
                         }
                          */ 
                            #endregion

                            responseText = exception.Message;
                            context.Response.StatusCode = statusCode > 0 ? statusCode : (int)HttpStatusCode.BadRequest;
                        }
                        var error = new ErrorDetails()
                        {
                            StatucCode = context.Response.StatusCode,
                            ErrorCode = errorCode,
                            Message = responseText,
                        }.ToString();

                        await context.Response.WriteAsync(error, Encoding.UTF8)
                        .AnyContext();

                        #endregion
                    }
                    );
            });
        }
            private static async Task LogExceptionAsync(ILogger logger, 
            HttpContext context, Exception exception, string ApplicationName)
        {
            var sb = new StringBuilder();
            var logMessage = new LogMessage
            {
                LogLevel = LogEventLevel.Error,
                ApplicationName = ApplicationName,
            };
            #region HandledExceptionsSwitchCase
                switch (exception.GetBaseException())
            {
                case AhmadException ahmadException:
                    logMessage.LogLevel = LogEventLevel.Warning;
                    break;
                case UnauthorizedResultException unauthorizedResultException:
                    logMessage.LogLevel = LogEventLevel.Information;
                    break;
                case TrainingException trainingException:
                    logMessage.LogLevel = LogEventLevel.Fatal;
                    break;
                default:
                    logMessage.LogLevel = LogEventLevel.Error;
                    break;
            } 
            #endregion
            #region HandledExceptionsIfStatement
            //if (HandledExceptions.Contains(exception.GetType().FullName))
            //{
            //    logMessage.LogLevel = LogEventLevel.Information;

            //    if (exception.GetType().FullName.Equals(typeof(AhmadException).FullName))
            //    {
            //        logMessage.LogLevel = LogEventLevel.Fatal;
            //    }
            //}
            #endregion
            while (exception != null) 
            {
                sb.Append($"{exception.Message}{Environment.NewLine}StackTrace:{exception.StackTrace}");
                exception = exception.InnerException;
            }
            logMessage.Message = sb.ToString();

            if(context != null)
            {
                
                logMessage.RequestPath = context.Request?.Path;
                logMessage.UserId = int.Parse(context.User.Claims.
                    FirstOrDefault(x => x.Type == "Id")?.Value ?? "-1");
                logMessage.UserEmail = context.User.Claims.
                    FirstOrDefault(x=> x.Type ==
                    "Email")?.Value ?? "guest";


            }

            await logger.LogMessageAsync(logMessage).AnyContext();

        }
    }
}
