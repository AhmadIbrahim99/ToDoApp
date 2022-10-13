using ClassLibrary.Common.Exceptions;
using ClassLibrary.Core.Managers.Interfaces;
using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Core.Managers
{
    public class CommonManager : ICommonManager
    {
        private readonly ToDoDBContext _cotnext;
        public CommonManager(ToDoDBContext cotnext)
        {
            _cotnext = cotnext;
        }

        public ApplicationUser GetUserRole(ApplicationUser userIN)=>_cotnext.ApplicationUsers.FirstOrDefault(a => a.Id == userIN.Id) ?? throw new AhmadException(401, "Invalid");
        
    }
}
