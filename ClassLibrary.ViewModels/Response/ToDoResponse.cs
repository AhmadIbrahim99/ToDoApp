using ClassLibrary.Common.Extenstions;
using ClassLibrary.ViewModels.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ViewModels.Response
{
    public class ToDoResponse
    {
        public PagedResult<ToDoVM> ToDoVM { get; set; }
        public Dictionary<int, UserVM> User { get; set; }
    }
}
