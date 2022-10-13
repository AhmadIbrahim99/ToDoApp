using ClassLibrary.Common.Attributes;
using ClassLibrary.Core.Managers.Interfaces;
using ClassLibrary.ViewModels.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.API.Controllers
{
    public class UserController : ApiBaseController
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [MyAuthorization("Admin")]
        public IActionResult Get() => Ok(LoggedInUser) ?? Ok(_userManager.GetAll());

        [HttpPost("Register")]
        public IActionResult Post([FromBody] RegisterVM userRegister) => Ok(_userManager.Register(userRegister));

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LogInVM userLogin) => Ok(_userManager.Login(userLogin));

        [HttpGet("fileretrive/profilepic")]
        public IActionResult Retrive(string fileName) => File(_userManager.Retrive(fileName), "iamge/jpeg", fileName);

        [HttpDelete("Delete/User")]
        [MyAuthorization("Admin")]
        public IActionResult Delete(int id)
        {
            _userManager.DeleteUser(id, LoggedInUser);
            return Ok();
        }

        [HttpPut("Update")]
        [Authorize]
        public IActionResult Update(UserVM request) => Ok(_userManager.Update(request, LoggedInUser));


    }
}
