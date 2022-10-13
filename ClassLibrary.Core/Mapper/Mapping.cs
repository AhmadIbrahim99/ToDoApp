using AutoMapper;
using ClassLibrary.Common.Extenstions;
using ClassLibrary.Models;
using ClassLibrary.ViewModels.ViewModel;

namespace ClassLibrary.Core.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ApplicationUser, UserVM>().ReverseMap();
            CreateMap<ApplicationUser, RegisterVM>().ReverseMap();
            CreateMap<ToDo, ToDoVM>().ReverseMap();
            CreateMap<PagedResult<ToDo>, PagedResult<ToDoVM>>().ReverseMap();

        }
    }
}
