using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ViewModels.ViewModel
{
    public class LogInVM
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Passwod { get; set; }
    }
}
