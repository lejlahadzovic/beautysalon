using BeautySalon.Models;
using BeautySalon.Contracts;
namespace BeautySalon.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Insert(UserVM insert);
        Task<User> Update(string email, UserUpdateVM update);
        Task<User> GetAll();
        Task<List<UserVM>> GetUsers();
        Task<User> GetByID(int id);
        Task<User> CheckResetCode(string code);
        Task<User> ResetPassword(ResetPasswordVM model);
        Task<User> CheckEmail(string Email);
        Task<User> Login(UserLoginVM loginUser);
        void ChangeResetPasswordCode(User user, string code);
        void ChangePassword(User entity, ChangePasswordVM model);
    }
}
