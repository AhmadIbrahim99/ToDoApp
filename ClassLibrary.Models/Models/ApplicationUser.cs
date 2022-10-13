using System;
using System.Collections.Generic;

#nullable disable

namespace ToDoApp.API.Models
{
    public partial class ApplicationUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public bool Archived { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImageString { get; set; }
    }
}
