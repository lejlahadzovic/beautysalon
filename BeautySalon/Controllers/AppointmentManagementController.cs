using AutoMapper;
using BeautySalon.Constants;
using BeautySalon.Contracts;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalon.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class AppointmentManagementController : Controller
    {
        protected readonly IAppointmentService _appointmentService;
        protected IMapper _mapper { get; set; }
        public async Task<IActionResult> Index()
        {
            List<AppointmentVM> appointments = await _appointmentService.GetAppointments();
            return View(appointments);
        }
    }
}
