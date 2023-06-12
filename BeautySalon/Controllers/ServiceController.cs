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
    }
}
