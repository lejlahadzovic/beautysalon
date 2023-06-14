using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalon.Controllers
{
    public class CatalogManagementController : Controller
    {
        protected new readonly ICatalogService _catalogService;
        public CatalogManagementController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }
        public async Task<IActionResult> Index()
        {
            var catalogs = await _catalogService.GetAll();
            return View(catalogs);
        }
    }
}
