using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ViewModels.ViewModel
{
    public class UserVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string ImageString { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
