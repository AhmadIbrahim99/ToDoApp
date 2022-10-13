using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Core.Managers.Interfaces
{
    public interface IRoleManager : IManager
    {
        bool CheckAccess(int Id, List<string> Permissions);
    }
}
