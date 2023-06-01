using BeautySalon.Constants;
using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Helper;
using BeautySalon.Models;
using BeautySalon.Services.Implementations;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace BeautySalon.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        protected new readonly IBaseService<UserVM, User, UserVM, UserVM> _service;
        protected new readonly UserService _userService;
        public UserController(ApplicationDbContext dbContext, IBaseService<UserVM, User, UserVM, UserVM> service, UserService userService)
        {
            _dbContext = dbContext;
            _service = service;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Register()
        {
            UserVM user = new UserVM();
            user.BirthDate = DateTime.Now;
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Register(UserVM newUser)
        {
            if (!ModelState.IsValid)
            {  
                return View(newUser);
            }
       
            var user = await _service.Insert(newUser);
            if(user != null)
            {
                return RedirectToAction("Login", "User");
            }

            TempData["message"] = "User is not registered successfully.";
            return View(newUser);
        }

        [HttpGet]
        public ActionResult Login()
        {
            UserLoginVM user = new UserLoginVM();
        
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserLoginVM loginUser)
        {
            if (!ModelState.IsValid)
            {
                return View(loginUser);
            }

            var entity=await _userService.Login(loginUser);

            if (entity == null)
            {
                ViewBag.Message = Messages.INVALID_CREDIENTIAL;
                return View(loginUser);
            }
         
            var claims = new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier, Convert.ToString(entity.Id)),
            new Claim(ClaimTypes.Email, entity.Email),
            };

            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = loginUser.RememberLogin
            });

            return RedirectToAction("Index","Catalog");
        }

        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "User");
        }

    }
}
