using System;
using System.Collections.Generic;

#nullable disable

namespace ClassLibrary.Models
{
    public partial class ApplicationUser
    {
        public ApplicationUser()
        {
            ToDos = new HashSet<ToDo>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool Archived { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public DateTime? DeletedAt { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImageString { get; set; } = "";
        public virtual ICollection<ToDo> ToDos { get; set; }
    }
}
