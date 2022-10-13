using ClassLibrary.ViewModels.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ViewModels.Response
{
    public class LogedInResponseVM
    {
        public string Token { get; set; }
        public UserVM Result { get; set; }
    }
}
