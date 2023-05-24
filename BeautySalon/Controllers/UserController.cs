using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Models;
using Microsoft.AspNetCore.Mvc;
using BeautySalon.Helper;
using System.Security.Cryptography;
using System.Text;

namespace BeautySalon.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult Register()
        {
            UserVM user = new UserVM();
            return View(user);
        }

        [HttpPost]
        public ActionResult Register(UserVM newUser)
        { 
            if (!ModelState.IsValid)
            {
                return View(newUser);
            }
            var isEmailAlreadyExists = _dbContext.Users.Any(x => x.Email == newUser.Email);
            if (isEmailAlreadyExists)
            {
                ModelState.AddModelError("Email", "User with this email already exists");
                return View(newUser);
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
