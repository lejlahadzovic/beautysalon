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
        public async Task<IActionResult> Index(int catalogId)
        {
            var services = await _serviceService.GetServicesByCatalogId(catalogId); 
            
            return View(services);
        }

        public async Task<IActionResult> Search(string name, int catalogId)
        {
            var services = await _serviceService.GetServicesByName(name,catalogId);

            return View(services);
        }

    }
}
