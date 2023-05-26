using AutoMapper;
using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Helper;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Services.Implementations
{
    public class UserService:BaseService<UserVM,User,UserVM, UserVM> 
    {

        public UserService(ApplicationDbContext dbContext,IMapper mapper):base(dbContext, mapper)
        {
        }
        public override async Task<User> Insert(UserVM insert)
        {
            var set = _dbContext.Set<User>();
            User entity = _mapper.Map<User>(insert);
            entity.PasswordSalt = PasswordHelper.GenerateSalt();
            entity.PasswordHash = PasswordHelper.GenerateHash(entity.PasswordSalt, insert.Password);
            entity.RoleId = 1;   // TODO : find id of role 'Customer' 
            set.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

    }
}
