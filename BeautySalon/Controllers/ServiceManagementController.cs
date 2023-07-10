using AutoMapper;
using BeautySalon.Constants;
using BeautySalon.Contracts;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace BeautySalon.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class ServiceManagementController : Controller
    {
        protected new readonly IServiceService _serviceService;
		protected new readonly ICatalogService _catalogService;
		protected IMapper _mapper { get; set; }

		public ServiceManagementController(IServiceService serviceService, IMapper mapper, ICatalogService catalogService)
		{
			_serviceService = serviceService;
			_mapper = mapper;
			_catalogService = catalogService;
		}

		public async Task<IActionResult> Index(string name, int catalogId)
        {
            var catalogsList = await _catalogService.GetCatalogs();
            ViewBag.Catalogs = new SelectList(catalogsList, "Id", "Title");
            var services = await _serviceService.Get(name, catalogId);
            return View(services);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Catalog> catalogsList = await _catalogService.GetCatalogs();

            ViewBag.Catalogs = new SelectList(catalogsList, "Id", "Title");
            ServiceVM service = new ServiceVM();

            return View("Edit",service);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ServiceVM newService)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", newService);
            }
            var service = await _serviceService.Insert(newService);
            
            return RedirectToAction("Edit", new { id = service.Id });
		}

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            List<Catalog> catalogsList = await _catalogService.GetCatalogs();

            ViewBag.Catalogs = new SelectList(catalogsList, "Id", "Title");
            var existingService = await _serviceService.GetServiceById(id);
            var service =_mapper.Map<Service,ServiceVM>(existingService);

            return View(service);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ServiceVM newService)
        {
            if (!ModelState.IsValid)
            {
                return View(newService);
            }
            var service = await _serviceService.Update(newService);
            if (service != null)
            {
                List<Catalog> catalogsList = await _catalogService.GetCatalogs();
                ViewBag.Catalogs = new SelectList(catalogsList, "Id", "Title");
                return View(newService);
            }

            return View(newService);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var appointmentsCount = await _serviceService.CheckAppointment(id);
            if (appointmentsCount > 0)
            {
                TempData["message"] = Messages.EDIT_DELETE_SERVICE;
                return RedirectToAction("Edit", new { id = id });
            }
            else 
            {
               var service=_serviceService.Delete(id);
               return RedirectToAction("Index");
            }
        }
    }
}