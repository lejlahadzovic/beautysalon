using AutoMapper;
using BeautySalon.Constants;
using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;

namespace BeautySalon.Controllers
{
    [Authorize(Roles=Roles.ADMIN)]
    public class CatalogManagementController : Controller
    {
        protected new readonly ICatalogService _catalogService;
        protected IMapper _mapper { get; set; }
        protected new readonly IWebHostEnvironment _hostEnvironment;

        public CatalogManagementController(ICatalogService catalogService, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _catalogService = catalogService;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var catalogs = await _catalogService.GetAll();
            return View(catalogs);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CatalogVM catalog = new CatalogVM();
            return View("Edit", catalog);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CatalogVM newCatalog, IFormFile imgfile)
        {
            if (imgfile != null && imgfile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string imageFileName = $"{Path.GetFileNameWithoutExtension(imgfile.FileName)}{newCatalog.Title}" +
                   $"{Path.GetExtension(imgfile.FileName)}";
                string filePath = Path.Combine(uploadsFolder, imageFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imgfile.CopyToAsync(fileStream);
                }
                newCatalog.ImageFileString = imageFileName;
            }
            var catalog = await _catalogService.Insert(newCatalog);
            if(catalog != null)
            {
                return RedirectToAction("Index");
            }
            return View("Edit", newCatalog);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int catalogId)
        {
            if(catalogId!=0)
            {
                var existingCatalog = await _catalogService.GetById(catalogId);
                var catalog = _mapper.Map<CatalogVM>(existingCatalog);
                return View(catalog);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CatalogVM editedCatalog, IFormFile imgfile)
        {
            var existingCatalog = await _catalogService.GetById(editedCatalog.Id);
            if (imgfile != null && imgfile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string imageFileName = $"{Path.GetFileNameWithoutExtension(imgfile.FileName)}{editedCatalog.Title}" +
                    $"{Path.GetExtension(imgfile.FileName)}";
                var imgFileFromExistingCatalog = existingCatalog.ImageFileString;
                if (imgFileFromExistingCatalog == imageFileName)
                {
                    var fullPath = uploadsFolder +"\\"+ imageFileName;
                    if(System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                else
                {
                    var existingImage = uploadsFolder + "\\" + existingCatalog.ImageFileString;
                    if(System.IO.File.Exists(existingImage))
                    {
                        System.IO.File.Delete(existingImage);
                    }
                }
                string filePath = Path.Combine(uploadsFolder, imageFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imgfile.CopyToAsync(fileStream);
                }
                editedCatalog.ImageFileString = imageFileName;
            }
            var catalog = await _catalogService.Update(editedCatalog.Id, editedCatalog);
            if(catalog != null)
            {
                return RedirectToAction("Index");
            }
            return View(editedCatalog);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int catalogId)
        {

        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(Catalog catalog)
        {
            string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
            string imagesToDelete = $"*{catalog.Title}*";
            string[] imagesList = System.IO.Directory.GetFiles(uploadFolder, imagesToDelete);
            foreach(string image in imagesList)
            {
                System.IO.File.Delete(image);
            }
            if (catalog!=null)
            {
                await _catalogService.Remove(catalog);
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        public async Task<IActionResult> Search(string catalogName)
        {
            IEnumerable<CatalogVM> catalogs;
            if (!string.IsNullOrEmpty(catalogName))
            {
                catalogs = await _catalogService.SearchByName(catalogName);
            }
            else
            {
                catalogs = await _catalogService.GetAll();
            }
            return View("~/Views/CatalogManagement/Index.cshtml", catalogs);
        }
    }
}
