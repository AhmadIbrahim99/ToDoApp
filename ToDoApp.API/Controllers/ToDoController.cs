using ClassLibrary.Common.Attributes;
using ClassLibrary.Core.Managers.Interfaces;
using ClassLibrary.ViewModels.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.API.Controllers
{
    public class ToDoController : ApiBaseController
    {
        private readonly IToDoManager _manager;
        public ToDoController(IToDoManager manager)
        {
            _manager = manager;
        }

        [HttpGet("api/todos")]
        public IActionResult GetAll(int page = 1, int pageSize = 10, string searchText = "", string sortColumn = "", string sortDirection = "ascending") => Ok(_manager.GetToDos(page, pageSize, searchText, sortColumn, sortDirection));

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("api/Tasks")]
        public IActionResult GetAll(bool user,int page = 1, int pageSize = 10, string searchText = "", string sortColumn = "", string sortDirection = "ascending") => Ok(_manager.GetToDos(LoggedInUser,page, pageSize, searchText, sortColumn, sortDirection));

        [HttpGet("api/todo/{id}")]
        public IActionResult GetBlog(int id) => Ok(_manager.GetToDo(id));

        [HttpPut("api/todo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult PutBlog([FromBody]ToDoRequest request) => Ok(_manager.PutToDo(LoggedInUser, request));

        [HttpDelete("api/blog/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [MyAuthorization("Admin")]
        public IActionResult ArchivedBlog(int id)
        {
            _manager.ArchiveToDo(LoggedInUser, id);
            return Ok();
        }

    }
}
