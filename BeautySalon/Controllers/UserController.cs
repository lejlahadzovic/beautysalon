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
            if (!ModelState.IsValid)
            {
                return View(newUser);
            }
            var entity = await _userService.CheckEmail(newUser.Email);
            if (entity != null)
            {
                ModelState.AddModelError("Email", Messages.EMAIL_EXISTS_ERROR_MESSAGE);
                return View(newUser);
            }
            var user = await _userService.Insert(newUser);
            if (user != null)
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

            var entity = await _userService.Login(loginUser);

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
            Thread.CurrentPrincipal = principal;
            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = loginUser.RememberLogin
            });

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
                    string subject = "Your changed password";
                    string body = "Hi, We got request for reset your account password. Please click on the below link to reset your password" +
                    " '" + link + "' Reset Password link";

                    //Call send email methods.
                    EmailManager.SendEmail(subject, body, To);
                    _userService.ChangeResetPasswordCode(user, resetCode);
                }
            }

            return View();
        }

        public async Task<User> GetCurrentRole()
        {
            var userWithClaims = (ClaimsPrincipal)User;
            Claim CRole = userWithClaims.Claims.First(c => c.Type == ClaimTypes.Email);
            var email = CRole.Value;
            var entity = await _userService.CheckEmail(email);

            return entity;
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
            var entity = await GetCurrentRole();
            UserUpdateVM user = _mapper.Map<User, UserUpdateVM>(entity);

            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Profile(UserUpdateVM editUser)
        {
            if (!ModelState.IsValid)
            {
                return View(editUser);
            }

            var entity = await _userService.Update(editUser.Email, editUser);
            if (entity != null)
            {
                ViewBag.Message = "You have successfully updated your profile.";
                UserUpdateVM user=_mapper.Map<User, UserUpdateVM>(entity);
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
            
            var entity = await GetCurrentRole();
            if (entity != null)
            {
                var hash = PasswordHelper.GenerateHash(entity.PasswordSalt, editUser.OldPassword);
                if (hash == entity.PasswordHash)
                {
                    _userService.ChangePassword(entity, editUser);
                    return RedirectToAction("ChangePassword", "User");
                }
            }

            return View(editUser);
        }
    }
}