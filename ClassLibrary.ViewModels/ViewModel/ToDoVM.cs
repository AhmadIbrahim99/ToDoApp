using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ViewModels.ViewModel
{
    public class ToDoVM
    {
        public string Title { get; set; }
        public string ImageString { get; set; }
        public bool IsRead { get; set; }
        public string ContentToDo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        [Display(Name = "User Task")]
        public int UserIdTask { get; set; }
        [Display(Name = "User Creator")]
        public int UserId { get; set; }
    }
}
