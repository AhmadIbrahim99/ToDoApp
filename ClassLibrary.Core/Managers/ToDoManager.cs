using AutoMapper;
using ClassLibrary.Common.Exceptions;
using ClassLibrary.Core.Managers.Interfaces;
using ClassLibrary.Models;
using ClassLibrary.ViewModels.Request;
using ClassLibrary.ViewModels.Response;
using ClassLibrary.ViewModels.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary.Common.Extenstions;
using ClassLibrary.Common.Helper;

namespace ClassLibrary.Core.Managers
{
    public class ToDoManager : IToDoManager
    {
        private readonly ToDoDBContext _context;
        private readonly IMapper _map;
        public ToDoManager(ToDoDBContext context, IMapper mapper)
        {
            _context = context;
            _map = mapper;
        }

        public void ArchiveToDo(ApplicationUser currentUser, int id)
        {
            if (!currentUser.IsAdmin) throw new UnauthorizedResultException(401, "You have no permission to delete an ToDo!");
            var data = _context.ToDos.FirstOrDefault(b => b.Id == id)
                ?? throw new AhmadException(403, "Invalid ToDo id received");
            data.Archived = true;
            data.DeletedAt = DateTime.Now;
            _context.SaveChanges();
        }

        public ToDoVM GetToDo(int id)=>_map.Map<ToDoVM>(_context.ToDos.
                 Include("User")// ==  a=>a.User
                 .FirstOrDefault(b => b.Id == id) ??
                 throw new AhmadException(403, "Invalid ToDo id received"));

        public ToDoVM GetToDo(ApplicationUser currentUser,int id)
        {
          var data =  _context.ToDos.
                 Include("User")
                 .FirstOrDefault(b => b.Id == id && currentUser.Id == b.UserIdTask) ??
                 throw new AhmadException(403, "Invalid Task");

            data.IsRead = true;
            _context.SaveChanges();
            return _map.Map<ToDoVM>(data);
        }

        public ToDoResponse GetToDos(int page = 1, int pageSize = 10, string searchText = "", string sortColumn = "", string sortDirection = "ascending")
        {
            _context.IgnoreFilter = true;
            var queryRes = _context.ToDos
                .Where(a => string.IsNullOrWhiteSpace(searchText)
                || (a.Title.Contains(searchText) ||
                a.ContentToDo.Contains(searchText)));

            if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.ToLower().Equals("ascending"))
            {
                queryRes = queryRes.OrderBy(sortColumn);
            }
            else if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.ToLower().Equals("descending"))
            {
                queryRes = queryRes.OrderByDescending(sortColumn);
            }

            var res = queryRes.GetPaged(page, pageSize);

            var userids = res.Data
                .Select(a => a.UserId)
                .Distinct()
                .ToList();

            var users = _context.ApplicationUsers
                .Where(a => userids.Contains(a.Id))
                .ToDictionary(a => a.Id, x => _map.Map<UserVM>(x));

            var data = new ToDoResponse
            {
                ToDoVM = _map.Map<PagedResult<ToDoVM>>(res),
                User = users
            };
            data.ToDoVM.Sortable.Add("Title", "Title");
            data.ToDoVM.Sortable.Add("ContentToDo", "Content ToDo");
            data.ToDoVM.Sortable.Add("CreatedAt", "Created Date");
            data.ToDoVM.Sortable.Add("Id", "Id");
            return data;
        }

        public ToDoResponse GetToDos(ApplicationUser currentUser, int page = 1, int pageSize = 10, string searchText = "", string sortColumn = "", string sortDirection = "ascending")
        {
            var queryRes = _context.ToDos
                .Where(a => string.IsNullOrWhiteSpace(searchText)
                || (a.Title.Contains(searchText) ||
                a.ContentToDo.Contains(searchText))).
                Where(e => e.UserIdTask == currentUser.Id);


            if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.ToLower().Equals("ascending"))
            {
                queryRes = queryRes.OrderBy(sortColumn);
            }
            else if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.ToLower().Equals("descending"))
            {
                queryRes = queryRes.OrderByDescending(sortColumn);
            }

            var res = queryRes.GetPaged(page, pageSize);

            var userids = res.Data
                .Select(a => a.UserId)
                .Distinct()
                .ToList();

            var users = _context.ApplicationUsers
                .Where(a => userids.Contains(a.Id))
                .ToDictionary(a => a.Id, x => _map.Map<UserVM>(x));

            var data = new ToDoResponse
            {
                ToDoVM = _map.Map<PagedResult<ToDoVM>>(res),
                User = users
            };
            data.ToDoVM.Sortable.Add("Title", "Title");
            data.ToDoVM.Sortable.Add("ContentToDo", "Content ToDo");
            data.ToDoVM.Sortable.Add("CreatedAt", "Created Date");
            data.ToDoVM.Sortable.Add("Id", "Id");
            return data;
        }

        public ToDoVM PutToDo(ApplicationUser currentUser, ToDoRequest toDoRequest)
        {
            ToDo toDo = null;


            if (toDoRequest.Id > 0)
                {
                    toDo = _context.ToDos.FirstOrDefault(b => b.Id == toDoRequest.Id);
                    toDo.Title = toDoRequest.Title;
                    toDo.ContentToDo = toDoRequest.ContentToDo;

                }
                else
                {
                    toDo = _context.ToDos.Add(new ToDo
                    {
                        Title = toDoRequest.Title,
                        ContentToDo = toDoRequest.ContentToDo,
                        UserId = currentUser.Id,
                        UserIdTask = currentUser.Id
                    }
                    ).Entity;

                if (currentUser.IsAdmin && toDoRequest.UserIdTask != currentUser.Id && toDoRequest.UserIdTask > 0)
                {
                    _context.ToDos.Add(new ToDo
                    {
                        Title = toDoRequest.Title,
                        ContentToDo = toDoRequest.ContentToDo,
                        UserId = currentUser.Id,
                        UserIdTask = toDoRequest.UserIdTask
                    });
                }
            }
            var url = "";
            if (!string.IsNullOrWhiteSpace(toDoRequest.Iamge)) url = FileHelper.SaveImage(toDoRequest.Iamge, "todoimage");

            var baseUrl = "https://localhost:44322/";

            if (!string.IsNullOrWhiteSpace(url)) toDo.ImageString = @$"{baseUrl}/api/v1/user/filretrive/todopic?filename={url}";

            _context.SaveChanges();

            return _map.Map<ToDoVM>(toDo);
        }
    }
}
