using BeautySalon.Constants;
using BeautySalon.Contracts;
using BeautySalon.Models;
using BeautySalon.Services.Implementations;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeautySalon.Controllers
{
    public class AppointmentController : Controller
    {
        protected new readonly IUserService _userService;
        protected new readonly IAppointmentService _appointmentService;
        
        public AppointmentController(IUserService userService, IAppointmentService appointmentService)
        {
            _userService = userService;
            _appointmentService = appointmentService;
        }
        public async Task<User> GetCurrentUser()
        {
            var userWithClaims = (ClaimsPrincipal)User;
            Claim CUser = userWithClaims.Claims.First(c => c.Type == ClaimTypes.Email);
            var email = CUser.Value;
            var user = await _userService.CheckEmail(email);

            return user;
        }

        public async Task<IActionResult> Index()
        {
            var user=await GetCurrentUser();
            var appointments =await _appointmentService.GetAppointmentsByUser(user);
            return View(appointments);
        }

        [HttpPost]
        public async Task<ActionResult> Create(int serviceId, DateTime dateTime)
        {
            var user = await GetCurrentUser();
            var newAppointment=_appointmentService.Create(user,dateTime,serviceId);
            string message;
            if(newAppointment != null)
            {
               message = Messages.APPOINTMENT_CREATED;
               return RedirectToAction("Details", "Service", new { serviceId = serviceId, message = message }, null);
            }
            message = Messages.APPOINTMENT_NOT_CREATED;
            return RedirectToAction("Details", "Service", new { serviceId = serviceId, message = message }, null);
        }

        [HttpGet]
        public async Task<ActionResult> Cancel(int id)
        {
            var service =await _appointmentService.Cancel(id);

            return RedirectToAction("Index");
        }
    }
}
