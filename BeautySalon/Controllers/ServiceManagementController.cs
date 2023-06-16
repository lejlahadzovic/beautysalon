using AutoMapper;
using BeautySalon.Constants;
using BeautySalon.Contracts;
using BeautySalon.Models;
using BeautySalon.Services.Implementations;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalon.Controllers
{
    public class ServiceManagementController : Controller
    {
        protected new readonly IServiceService _serviceService;
        protected IMapper _mapper { get; set; }

        public ServiceManagementController(IServiceService serviceService, IMapper mapper)
        {
            _serviceService = serviceService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string name)
        {
            var services = await _serviceService.GetAll(name);
            return View(services);
        }

        [HttpGet]
        public ActionResult Create()
        {
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
            if (service != null)
            {
                return RedirectToAction("Index", "Service");        
            }

            return View("Edit",newService);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
           
            var existingService = await _serviceService.GetServicesById(id);
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
            var service = await _serviceService.Update(newService.Id,newService);
            if (service != null)
            {
                return View();
            }

            return View(newService);
        }
    }
}