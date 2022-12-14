using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Core.Managers.Interfaces
{
    public interface ICommonManager : IManager
    {
        ApplicationUser GetUserRole(ApplicationUser user);
    }
}
