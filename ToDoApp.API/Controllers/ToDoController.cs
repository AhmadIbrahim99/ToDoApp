using ClassLibrary.Core.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.API.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IToDoManager _manager;
        public ToDoController(IToDoManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok();
    }
}
