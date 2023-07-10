using AutoMapper;
using BeautySalon.Constants;
using BeautySalon.Contracts;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace BeautySalon.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class CatalogManagementController : Controller
    {
        protected readonly ICatalogService _catalogService;
        protected IMapper _mapper { get; set; }

        public CatalogManagementController(ICatalogService catalogService, IMapper mapper)
        {
            _catalogService = catalogService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string catalogName)
        {
            var catalogs = await _catalogService.GetCatalogs(catalogName);
            return View(catalogs);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CatalogVM catalog = new CatalogVM();
            return View("Edit", catalog);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CatalogVM newCatalog)
        {
            ModelState.Remove("imgfile");
            if(ModelState.IsValid)
            {
                var catalog = await _catalogService.Insert(newCatalog);
                if (catalog != null) 
                { 
                    return RedirectToAction("Edit", new { catalogId = catalog.Id }); 
                }
            }
            return View("Edit", newCatalog);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int catalogId)
        {
            if (catalogId != 0)
            {
                var existingCatalog = await _catalogService.GetById(catalogId);
                var catalog = _mapper.Map<CatalogVM>(existingCatalog);
                return View(catalog);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CatalogVM editedCatalog)
        {
            ModelState.Remove("imgfile");
            if(ModelState.IsValid)
            {
                var catalog = await _catalogService.Update(editedCatalog.Id, editedCatalog);
                var check = catalog != null ? _mapper.Map<CatalogVM>(catalog) : editedCatalog;
                return View(check);
            }
            return View(editedCatalog);
        }

        public async Task<IActionResult> Delete(int catalogId)
        {
            var catalog = await _catalogService.GetById(catalogId);
            if (catalog != null)
            {
                await _catalogService.Remove(catalog);
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
