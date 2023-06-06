﻿using BeautySalon.Models;
using BeautySalon.Contracts;
namespace BeautySalon.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Insert(UserVM insert);
        Task<User> GetAll();
        Task<User> GetByID(int id);
        Task<User> CheckResetCode(string code);
        Task<User> ResetPassword(ResetPasswordVM model);
        Task<User> CheckEmail(string Email);
        void ChangeResetPasswordCode(User user, string code);

    }
}
