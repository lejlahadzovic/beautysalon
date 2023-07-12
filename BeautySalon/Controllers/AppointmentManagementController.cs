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

        public async Task<IActionResult> Index(int userId, int catalogId, int serviceId, bool isApproved)
        {
            var usersList =await _userService.GetUsers();
            ViewBag.Users = new SelectList(usersList, "Id", "FullName");

            var catalogsList = await _catalogService.GetCatalogs();
            ViewBag.Catalogs = new SelectList(catalogsList, "Id", "Title");

            var servicesList = await _serviceService.Get("",0);
            ViewBag.Services = new SelectList(servicesList, "Id", "Name");
            
            var appointments = await _appointmentService.GetAppointments(userId, catalogId, serviceId, isApproved);
            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int appointmentId)
        {
            if (appointmentId != 0)
            {
                var appointment = await _appointmentService.GetById(appointmentId);
                return View(_mapper.Map<AppointmentVM>(appointment));
            }
            return NotFound();
        }
    }
}
