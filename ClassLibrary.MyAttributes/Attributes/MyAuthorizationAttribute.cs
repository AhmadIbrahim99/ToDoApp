using ClassLibrary.Common.Exceptions;
using ClassLibrary.Core.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;


namespace ClassLibrary.Common.Attributes
{
    public class MyAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _permissions;
        public MyAuthorizationAttribute()
        {

        }
        public MyAuthorizationAttribute(string permissions)
        {
            _permissions = permissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var roleManager = context.HttpContext.RequestServices.GetService(typeof(IRoleManager)) as IRoleManager;
                var ClaimId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value;

                if (roleManager.CheckAccess(int.TryParse(ClaimId, out int Id) ? Id : -1, _permissions.Split(',').ToList()))
                {
                    return;
                }
                throw new AhmadException("UnAuthorized");
            }
            catch (RetryLimitExceededException ex)
            {
                throw new AhmadException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new AhmadException(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                throw new AhmadException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new UnauthorizedResultException(new UnauthorizedResult().StatusCode, ex.Message);
                //Log.Logger.Information(ex.Message);
                //context.Result = new UnauthorizedResult();
                //return;
            }

        }
    }
}
