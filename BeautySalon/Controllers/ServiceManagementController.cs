using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalon.Controllers
{
    public class ServiceManagementController : Controller
    {
        protected new readonly IServiceService _serviceService;

        public ServiceManagementController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _serviceService.GetAll();
            return View(services);
        }
    }
}