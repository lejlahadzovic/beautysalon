using BeautySalon.Constants;
using BeautySalon.Context;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Controllers
{
    public class ServiceController : Controller
    {
        protected new readonly IServiceService _serviceService;
        
        public ServiceController(IServiceService serviceService)
        {
            _serviceService= serviceService;
        }
        
        public async Task<IActionResult> Index(int catalogId, string name)
        {
            var services = await _serviceService.GetServices(catalogId, name); 
            return View(services);
        }

        public async Task<IActionResult> Details(int serviceId,string message)
        {
            TempData["message"] = message;
            var services = await _serviceService.GetServiceById(serviceId);
            return View(services);
        }
    }
}
