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
using Azure.Core;
using System.Net.Mail;
using System.Net;
using System.IO;
using System;

namespace BeautySalon.Services.Implementations
{
    public class UserService : IUserService
    {
        protected ApplicationDbContext _dbContext;
        protected IMapper _mapper { get; set; }
        private const string RoleName = "Customer";
       
        public UserService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public virtual async Task<User> GetByID(int id)
        {
            var entity = await _dbContext.Users.FindAsync(id);

            return _mapper.Map<User>(entity);
        }

        public virtual async Task<User> GetAll()
        {

            var list = await _dbContext.Users.ToListAsync();

            return _mapper.Map<User>(list);
        }
        public async Task<User> Insert(UserVM insert)
        {
         
            var set = _dbContext.Users;
            User entity = _mapper.Map<User>(insert);
            entity.PasswordSalt = PasswordHelper.GenerateSalt();
            entity.PasswordHash = PasswordHelper.GenerateHash(entity.PasswordSalt, insert.Password);
            entity.RoleId = _dbContext.Roles.Where(x => x.Name.Contains(RoleName)).ToList().Select(x=>x.Id).First();
            set.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
        public async Task<User> ResetPassword(ResetPasswordVM model)
        {
            var entity = await CheckResetCode(model.ResetCode);
            if (entity != null)
            {
                entity.PasswordSalt = PasswordHelper.GenerateSalt();
                entity.PasswordHash = PasswordHelper.GenerateHash(entity.PasswordSalt, model.NewPassword);
                entity.ResetPasswordCode = "";
                _dbContext.SaveChanges();
            }

            return entity;
        }
        public async Task<User> CheckEmail(string Email)
        {
            var entity = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == Email);

            return entity;
        }

        public async Task<User> CheckResetCode(string code)
        {
            var entity = await _dbContext.Users.FirstOrDefaultAsync(x => x.ResetPasswordCode == code);

            return entity;
        }
        public void ChangeResetPasswordCode(User user,string code)
        {
            user.ResetPasswordCode = code;
            _dbContext.SaveChanges();
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
