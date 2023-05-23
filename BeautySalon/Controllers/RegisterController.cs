using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Models;
using Microsoft.AspNetCore.Mvc;
using BeautySalon.Helper;
using System.Security.Cryptography;
using System.Text;

namespace BeautySalon.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public RegisterController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserVM newUser)
        { 
            if (!ModelState.IsValid)
            {
                return View();
            }

            User user = new User();
            _dbContext.Add(user);
             user.FirstName = newUser.FirstName;
                user.LastName = newUser.LastName;
                user.Email = newUser.Email;
                user.PhoneNumber = newUser.PhoneNumber;
                user.Gender = newUser.Gender;
                user.BirthDate = newUser.BirthDate;
                user.PasswordSalt = PasswordHelper.GenerateSalt();
                user.PasswordHash = PasswordHelper.GenerateHash(user.PasswordSalt, newUser.Password);
                _dbContext.SaveChanges();
          
            return Ok();
        }
    }
}
