using ClassLibrary.Models;
using ClassLibrary.ViewModels.Response;
using ClassLibrary.ViewModels.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Core.Managers.Interfaces
{
    public interface IUserManager : IManager
    {
        List<UserVM> GetAll();
        LogedInResponseVM Register(RegisterVM userRegister);
        LogedInResponseVM Login(LogInVM userLogin);
        UserVM Update(UserVM request, ApplicationUser currentUser);
        byte[] Retrive(string fileName);
        void DeleteUser(int id, ApplicationUser currentUserFromRequest);
    }
}
