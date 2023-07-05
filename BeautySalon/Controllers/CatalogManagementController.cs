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
    public class CatalogManagementController : Controller
    {
        protected readonly ICatalogService _catalogService;
        protected IMapper _mapper { get; set; }

        public CatalogManagementController(ICatalogService catalogService, IMapper mapper)
        {
            _catalogService = catalogService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int pg = 1)
        {
            var catalogs = await _catalogService.GetAll();
            var pagination = Pagination(pg, catalogs);
            return View(pagination);
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
            if (!ModelState.IsValid)
            {
                return PartialView("Edit", newCatalog);
            }
            var catalog = await _catalogService.Insert(newCatalog);
            if (catalog != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Edit");
            }
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
            if (!ModelState.IsValid)
            {
                return PartialView(editedCatalog);
            }
            var catalog = await _catalogService.Update(editedCatalog.Id, editedCatalog);
            if (catalog != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
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

        public async Task<IActionResult> Search(string catalogName, int pg = 1)
        {
            List<CatalogVM> catalogs;
            if (!string.IsNullOrEmpty(catalogName))
            {
                catalogs = await _catalogService.SearchByName(catalogName);
            }
            else
            {
                catalogs = await _catalogService.GetAll();
            }
            var pagination = Pagination(pg, catalogs);
            return View("Index", catalogs);
        }

        private List<CatalogVM> Pagination(int pg, List<CatalogVM> catalogs)
        {
            const int pageSize = 10;
            if (pg < 1) { pg = 1; }
            int recsCount = catalogs.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = catalogs.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return data;
        }
    }
}
