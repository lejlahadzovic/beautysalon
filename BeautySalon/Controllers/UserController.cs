using BeautySalon.Constants;
using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Helper;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalon.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        protected new readonly IBaseService<UserVM, User, UserVM, UserVM> _service;
        public UserController(ApplicationDbContext dbContext, IBaseService<UserVM, User, UserVM, UserVM> service)
        {
            _dbContext = dbContext;
            _service = service;
        }

        [HttpGet]
        public ActionResult Register()
        {
            UserVM user = new UserVM();
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Register(UserVM newUser)
        {
            if (!ModelState.IsValid)
            {
                return View(newUser);
            }
            var isEmailAlreadyExists = _dbContext.Users.Any(x => x.Email == newUser.Email);
            if (isEmailAlreadyExists)
            {
                ModelState.AddModelError("Email", Messages.EMAIL_EXISTS_ERROR_MESSAGE);
                return View(newUser);
            }

            var user = await _service.Insert(newUser);
            if(user != null)
            {
                return RedirectToAction("Index", "Catalog");  // TODO: Redirect to Login page
            }

            // TODO: Add message that user is not registered
            return View(newUser);
        }
    }
}
