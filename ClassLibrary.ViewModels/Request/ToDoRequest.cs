using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ViewModels.Request
{
    public class ToDoRequest
    {
        public string Title { get; set; }
        public string ImageString { get; set; }
        public string ContentToDo { get; set; }
        public int UserIdTask { get; set; }
    }
}
