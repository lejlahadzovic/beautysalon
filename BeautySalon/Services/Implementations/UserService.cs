using AutoMapper;
using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Helper;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BeautySalon.Constants;

namespace BeautySalon.Services.Implementations
{
    public class UserService : BaseService<UserVM, User, UserVM, UserVM>
    {
        private const string RoleName = "Customer";
        public UserService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
        public override async Task<User> Insert(UserVM insert)
        {
         
            var set = _dbContext.Set<User>();
            User entity = _mapper.Map<User>(insert);
            entity.PasswordSalt = PasswordHelper.GenerateSalt();
            entity.PasswordHash = PasswordHelper.GenerateHash(entity.PasswordSalt, insert.Password);
            entity.RoleId = _dbContext.Roles.Where(x => x.Name.Contains(RoleName)).ToList().Select(x=>x.Id).First();
            set.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
        public async Task<User> CheckEmail(UserVM loginUser)
        {
           var entity = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == loginUser.Email);

            return entity;
        }

        [HttpPost]
        public async Task<User> Login(UserLoginVM loginUser)
        {
            var entity = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == loginUser.Email);

            if (entity == null)
            {
                return null;
            }
            var hash = PasswordHelper.GenerateHash(entity.PasswordSalt, loginUser.Password);

            if (hash != entity.PasswordHash)
            {
                return null;
            }
            
            return entity;
        }
    }
}
