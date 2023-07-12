using BeautySalon.Constants;
using BeautySalon.Contracts;
using BeautySalon.Models;
using BeautySalon.Services.Implementations;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BeautySalon.Controllers
{
    public class AppointmentController : Controller
    {
        protected new readonly IUserService _userService;
        protected new readonly IAppointmentService _appointmentService;
        protected new readonly ICatalogService _catalogService;
        protected new readonly IServiceService _serviceService;

        public AppointmentController(IUserService userService, IAppointmentService appointmentService, ICatalogService catalogService, IServiceService serviceService)
        {
            _userService = userService;
            _appointmentService = appointmentService;
            _catalogService = catalogService;
            _serviceService = serviceService;
        }
        public async Task<User> GetCurrentUser()
        {
            var userWithClaims = (ClaimsPrincipal)User;
            Claim CUser = userWithClaims.Claims.First(c => c.Type == ClaimTypes.Email);
            var email = CUser.Value;
            var user = await _userService.CheckEmail(email);

            return user;
        }

        public async Task<IActionResult> Index(AppointmentSearchVM appointmentSearch)
        {
            var catalogsList = await _catalogService.GetCatalogs();
            ViewBag.Catalogs = new SelectList(catalogsList, "Id", "Title");
            var serviceList = await _serviceService.Get(null,0);
            ViewBag.Services = new SelectList(serviceList, "Id", "Name");
            var user=await GetCurrentUser();
            var appointments = await _appointmentService.SearchAppointments(user.Id, appointmentSearch);
            return View(appointments);
        }

        [HttpPost]
        public async Task<ActionResult> Create(int serviceId, DateTime dateTime)
        {
            var user = await GetCurrentUser();
            var newAppointment=_appointmentService.Create(user.Id,dateTime,serviceId);
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
            if(service != null) 
            {
               TempData["message"] = @Messages.APPOINTMENT_CANCELED;
               return RedirectToAction("Index");
            }
            
            TempData["message"] = @Messages.APPOINTMENT_NOT_CANCELED;
            return RedirectToAction("Index");
        }
    }
}
