using ClassLibrary.Models;
using ClassLibrary.ViewModels.Request;
using ClassLibrary.ViewModels.Response;
using ClassLibrary.ViewModels.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Core.Managers.Interfaces
{
    public interface IToDoManager : IManager
    {
        ToDoResponse GetToDos(int page = 1, int pageSize = 10, string searchText = "", string sortColumn = "", string sortDirection = "ascending");
        ToDoResponse GetToDos(ApplicationUser currentUser,int page = 1, int pageSize = 10, string searchText = "", string sortColumn = "", string sortDirection = "ascending");
        ToDoVM GetToDo(int id);
        ToDoVM GetToDo(ApplicationUser currentUser, int id);
        ToDoVM PutToDo(ApplicationUser user, ToDoRequest toDoRequest);
        void ArchiveToDo(ApplicationUser user, int id);
    }
}
