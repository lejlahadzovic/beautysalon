using AutoMapper;
using BeautySalon.Constants;
using BeautySalon.Contracts;
using BeautySalon.Models;
using BeautySalon.Services.Implementations;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautySalon.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class AppointmentManagementController : Controller
    {
        protected readonly IAppointmentService _appointmentService;
        protected readonly IUserService _userService;
        protected readonly ICatalogService _catalogService;
        protected readonly IServiceService _serviceService;
        protected IMapper _mapper { get; set; }

        public AppointmentManagementController(IAppointmentService appointmentService, IUserService userService, 
            ICatalogService catalogService, IServiceService serviceService, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _userService = userService;
            _catalogService = catalogService;
            _serviceService = serviceService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int userId = 0, int catalogId = 0, int serviceId = 0, bool isApproved = false, bool isCanceled = false,
            DateTime? dateFrom = null, DateTime? dateTo = null, string dateRange = "")
        {
            var usersList =await _userService.GetUsers();
            ViewBag.Users = new SelectList(usersList, "Id", "FullName");

            var catalogsList = await _catalogService.GetCatalogs();
            ViewBag.Catalogs = new SelectList(catalogsList, "Id", "Title");

            var servicesList = await _serviceService.Get("",0);
            ViewBag.Services = new SelectList(servicesList, "Id", "Name");
            
            var appointments = await _appointmentService.GetAppointments(userId, catalogId, serviceId, isApproved, isCanceled, dateFrom, dateTo, dateRange);
            return View(appointments);
        }

        public async Task<IActionResult> ApproveAppointment(int appointmentId)
        {
            if (appointmentId != 0)
            {
                var appointment = await _appointmentService.GetById(appointmentId);
                _appointmentService.Approve(appointment);
                TempData["ApprovedMessage"] = Messages.APPOINTMENT_APPROVED_MESSAGE;
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
