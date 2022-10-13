using AutoMapper;
using ClassLibrary.Common.Exceptions;
using ClassLibrary.Common.Helper;
using ClassLibrary.Core.Managers.Interfaces;
using ClassLibrary.Models;
using ClassLibrary.ViewModels.Response;
using ClassLibrary.ViewModels.ViewModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Core.Managers
{
    public class UserManager : IUserManager
    {
        private readonly ToDoDBContext _context;
        private readonly IMapper _map;
        public UserManager(ToDoDBContext context, IMapper mapper)
        {
            _context = context;
            _map = mapper;
        }

        public void DeleteUser(int id, ApplicationUser currentUserFromRequest)
        {
            var currentUser = _map.Map<UserVM>(currentUserFromRequest);
            if (id == currentUserFromRequest.Id) throw new AhmadException("Are You Trying to deleter ur self?!!");

            var user = _context.ApplicationUsers.FirstOrDefault(a => a.Id == id) ?? throw new AhmadException(405, "User Not Found");

            user.Archived = true;
            user.DeletedAt = DateTime.Now;
            _context.SaveChanges();
        }

        public List<UserVM> GetAll() => _map.Map<List<UserVM>>(_context.ApplicationUsers.ToList());

        public LogedInResponseVM Login(LogInVM userLogin)
        {
            var user = _context.ApplicationUsers.FirstOrDefault
                (a => a.Email.ToLower().Equals(userLogin.Email.ToLower())) ?? throw new AhmadException(305, "Invalid User");

            if (user == null || !VerfyPassword(userLogin.Passwod, user.Password))
                throw new AhmadException(305, "Invalid Passwod or user");



            return new LogedInResponseVM { Token = $"Bearer {GenerateJWTToken(user)}", Result = _map.Map<UserVM>(user) };
        }

        public LogedInResponseVM Register(RegisterVM userRegister)
        {
            if (_context.ApplicationUsers.Any(a => a.Email.ToLower().Equals(userRegister.Email.ToLower()))) throw new AhmadException("User Already Exist");

            if (userRegister.Password != userRegister.ConfirmPassword) throw new AhmadException("Wrong Password");

            var hashPassword = HashPassword(userRegister.Password);
            var data = _context.Add(new ApplicationUser
            {
                Email = userRegister.Email.ToLower(),
                FirstName = userRegister.FirstName,
                LastName = userRegister.LastName,
                Password = hashPassword
            }).Entity;
            _context.SaveChanges();


            return new LogedInResponseVM { Token = $"Bearer {GenerateJWTToken(data)}", Result = _map.Map<UserVM>(data) };
        }

        public byte[] Retrive(string fileName)
        {
            var folderPath = Directory.GetCurrentDirectory();
            folderPath = $@"{folderPath}\{fileName}";
            var byteArray = File.ReadAllBytes(folderPath);
            return byteArray;
        }

        public UserVM Update(UserVM request, ApplicationUser currentUser)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.Id == currentUser.Id)
                ?? throw new AhmadException(400, "User Not Found");
            var url = "";
            if (!string.IsNullOrWhiteSpace(request.ImageString)) url = FileHelper.SaveImage(request.ImageString, "profileimage");

            var baseUrl = "https://localhost:44394/";

            if (!string.IsNullOrWhiteSpace(url)) user.ImageString = @$"{baseUrl}/api/v1/user/filretrive/profilepic?filename={url}";

            _context.SaveChanges();

            return _map.Map<UserVM>(user);
        }

        #region private

        private string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        private bool VerfyPassword(string password, string Hashedpassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, Hashedpassword);
        }
        private string GenerateJWTToken(ApplicationUser user)
        {

            var jwtKey = "#ahmad.key*&^vancy!@#$%^&*()_-+=-*/@@{}[]";
            var sercurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(sercurityKey, SecurityAlgorithms.HmacSha256);

            var issure = "test.com";
            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Iss, issure),
            new Claim(JwtRegisteredClaimNames.Aud, issure),
            new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName} {user.LastName}"),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("Email", user.Email),
            new Claim("Id", user.Id.ToString()),
            new Claim("DateOfJoining", user.CreatedAt.ToString("yyyy-MM-dd")),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            //var token = new JwtSecurityToken(claims:
            //    claims, expires: DateTime.Now.AddDays(12),
            //    signingCredentials:credentials);

            return new JwtSecurityTokenHandler().
                WriteToken(new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: credentials));
        }

        #endregion private
    }
}
