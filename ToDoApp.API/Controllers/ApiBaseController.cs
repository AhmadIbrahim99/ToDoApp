using ClassLibrary.Common.Exceptions;
using ClassLibrary.Core.Managers.Interfaces;
using ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace ToDoApp.API.Controllers
{
    public class ApiBaseController : Controller
    {
        private ApplicationUser _loggedInUser;
        protected ApplicationUser LoggedInUser
        {
            get
            {
                if (_loggedInUser != null) return _loggedInUser;

                Request.Headers.TryGetValue("Authorization", out StringValues Token);

                if (string.IsNullOrEmpty(Token)) { _loggedInUser = null; return _loggedInUser; }

                var ClaimId = User.Claims.FirstOrDefault(c => c.Type == "Id");

                if (ClaimId == null || !int.TryParse(ClaimId.Value, out int Id)) throw new AhmadException(401, "Invalid or Expired Token");

                var commonManager = HttpContext.RequestServices.GetService(typeof(ICommonManager)) as ICommonManager;
                _loggedInUser = commonManager.GetUserRole(new ApplicationUser { Id = Id });
                return _loggedInUser;

                //return new UserApp
                //{
                //    Id = Id,
                //    Name = "Ahmad",
                //    Email = "test@example.com"
                //};
            }
        }

        public ApiBaseController()
        {

        }
    }
}
