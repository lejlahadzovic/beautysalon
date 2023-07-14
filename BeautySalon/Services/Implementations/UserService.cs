using AutoMapper;
using BeautySalon.Constants;
using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Helper;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeautySalon.Services.Implementations
{
    public class UserService : IUserService
    {
        protected ApplicationDbContext _dbContext;
        protected IMapper _mapper { get; set; }
        private const string RoleName = Roles.CUSTOMER;
       
        public UserService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<User> GetByID(int id)
        {
            var entity = await _dbContext.Users.FindAsync(id);

            return _mapper.Map<User>(entity);
        }

        public async Task<User> GetAll()
        {
            var list = await _dbContext.Users.ToListAsync();

            return _mapper.Map<User>(list);
        }

        public async Task<List<UserVM>> GetUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            return _mapper.Map<List<UserVM>>(users);
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
   
        public async Task<User> Update(string email, UserUpdateVM update)
        {
            var entity = await CheckEmail(email);
            if (entity != null)
            {
                _mapper.Map(update, entity);
                _dbContext.Users.Update(entity);
                await _dbContext.SaveChangesAsync(); 
            }
         
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
                await _dbContext.SaveChangesAsync();
            }

            return entity;
        }

        public void ChangePassword(User entity,ChangePasswordVM model)
        {
            entity.PasswordSalt = PasswordHelper.GenerateSalt();
            entity.PasswordHash = PasswordHelper.GenerateHash(entity.PasswordSalt, model.NewPassword);
            _dbContext.SaveChangesAsync();
            
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

        public async Task<User> Login(UserLoginVM loginUser)
        {
            var entity = await _dbContext.Users.Include(u=>u.Role).FirstOrDefaultAsync(x => x.Email == loginUser.Email);
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
