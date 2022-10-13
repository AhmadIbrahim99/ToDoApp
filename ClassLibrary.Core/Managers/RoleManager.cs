using AutoMapper;
using ClassLibrary.Core.Managers.Interfaces;
using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Core.Managers
{
    public class RoleManager : IRoleManager
    {
        private readonly ToDoDBContext _context;
        private readonly IMapper _mapper;
        public RoleManager(ToDoDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public bool CheckAccess(int Id, List<string> Permissions)
        {
            var permission = false;
            if (Permissions.Any(x => x.Equals("Admin")))
                permission = _context.ApplicationUsers.
                Any(x => x.Id == Id && x.IsAdmin);

            return permission;
        }
    }
}
