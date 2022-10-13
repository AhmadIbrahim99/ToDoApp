using System;
using System.Collections.Generic;

#nullable disable

namespace ClassLibrary.Models
{
    public partial class ToDo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageString { get; set; } = "";
        public string ContentToDo { get; set; }
        public bool IsRead { get; set; } = false;
        public bool Archived { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public DateTime? DeletedAt { get; set; }
        public int UserIdTask { get; set; }
        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
