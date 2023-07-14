using BeautySalon.Constants;
using BeautySalon.Contracts;
using BeautySalon.Helper;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Controllers
{
    public class UserController : Controller
    {
        protected new readonly IUserService _userService;
        protected IMapper _mapper { get; set; }
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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
            ModelState.Remove("FullName");
            if (!ModelState.IsValid)
            {
                return View(newUser);
            }
            var existingUser = await _userService.CheckEmail(newUser.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", Messages.EMAIL_EXISTS_ERROR_MESSAGE);
                return View(newUser);
            }
            var user = await _userService.Insert(newUser);
            if (user != null)
            {
                return RedirectToAction("Login", "User");
            }

            TempData["message"] = Messages.REGISTER_NOT_SUCCESSFUL;
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

            var user = await _userService.Login(loginUser);

            if (user == null)
            {
                ViewBag.Message = Messages.INVALID_CREDIENTIAL;
                return View(loginUser);
            }

            SetLoginData(user, loginUser);
            
            return RedirectToAction("Index", "Catalog");
        }

        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "User");
        }
        
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordVM forgotPassword)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.CheckEmail(forgotPassword.Email);

                if (user != null)
                {
                    var To = user.Email;
                    //Generate password token
                    var resetCode = Guid.NewGuid().ToString();

                    //Create URL with an above token
                    var link = Url.Action("ResetPassword", "User", new { id = resetCode }, "https");
                    //HTML Template for Send email
                    string subject =Messages.EMAIL_MESSAGE_SUBJECT;
                    string body = Messages.EMAIL_MESSAGE_BODY_1 +
                    " '" + link + Messages.EMAIL_MESSAGE_BODY_2;

                    //Call send email methods.
                    EmailManager.SendEmail(subject, body, To);
                    _userService.ChangeResetPasswordCode(user, resetCode);
                    ViewBag.Message = Messages.EMAIL_SENT;
                }
            }

            return View();
        }

        public async Task<User> GetCurrentUser()
        {
            var userWithClaims = (ClaimsPrincipal)User;
            Claim CUser = userWithClaims.Claims.First(c => c.Type == ClaimTypes.Email);
            var email = CUser.Value;
            var user = await _userService.CheckEmail(email);

            return user;
        }

        public async void SetLoginData(User existingUser, UserLoginVM loginUser)
        {
            var claims = new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier, Convert.ToString(existingUser.Id)),
            new Claim(ClaimTypes.Email, existingUser.Email),
            new Claim(ClaimTypes.Role, existingUser.Role.Name)
            };

            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);
            Thread.CurrentPrincipal = principal;
            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = loginUser.RememberLogin
            });
        }

        [HttpGet]
        public async Task<ActionResult> ResetPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction("ForgotPassword", "User");
            }
            var user = await _userService.CheckResetCode(id);
            if (user != null)
            {
                ResetPasswordVM model = new ResetPasswordVM();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return RedirectToAction("ForgotPassword", "User");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.ResetPassword(model);
                if (user != null)
                {
                    return RedirectToAction("Login", "User");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Profile()
        {
            var user = await GetCurrentUser();
            UserUpdateVM updateUser = _mapper.Map<User, UserUpdateVM>(user);

            return View(updateUser);
        }

        [HttpPost]
        public async Task<ActionResult> Profile(UserUpdateVM editUser)
        {
            if (!ModelState.IsValid)
            {
                return View(editUser);
            }

            var updateUser = await _userService.Update(editUser.Email, editUser);
            if (updateUser != null)
            {
                ViewBag.Message = Messages.PROFILE_UPDATE_SUCCSESSFUL;
                UserUpdateVM user=_mapper.Map<User, UserUpdateVM>(updateUser);
                return View(user);
            }

            return View(editUser);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            ChangePasswordVM changePassword= new ChangePasswordVM();

            return View(changePassword);
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordVM editUser)
        {
            if (!ModelState.IsValid)
            {
                return View(editUser);
            }
            
            var user = await GetCurrentUser();
            if (user != null)
            {
                var hash = PasswordHelper.GenerateHash(user.PasswordSalt, editUser.OldPassword);
                if (hash == user.PasswordHash)
                {
                    _userService.ChangePassword(user, editUser);
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    return RedirectToAction("Login", "User");
                }
            }

            return View(editUser);
        }
    }
}