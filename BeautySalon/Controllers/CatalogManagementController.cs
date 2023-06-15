﻿using AutoMapper;
using BeautySalon.Constants;
using BeautySalon.Contracts;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalon.Controllers
{
    public class CatalogManagementController : Controller
    {
        protected new readonly ICatalogService _catalogService;
        protected IMapper _mapper { get; set; }

        public CatalogManagementController(ICatalogService catalogService, IMapper mapper)
        {
            _catalogService = catalogService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var catalogs = await _catalogService.GetAll();
            return View(catalogs);
        }

        public async Task<IActionResult> Search(string catalogTitle)
        {
            IEnumerable<Catalog> catalogs;
            if (!string.IsNullOrEmpty(catalogTitle))
            {
                catalogs = await _catalogService.SearchByName(catalogTitle);
            }
            else
            {
                catalogs = await _catalogService.GetAll();
            }
            return View("~/Views/CatalogManagement/Index.cshtml", catalogs);
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(int catalogId)
        {
            var catalog = await _catalogService.GetById(catalogId);
            return View(catalog);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CatalogVM catalogEdit)
        {
            if(!ModelState.IsValid)
            {
                return View(catalogEdit);
            }
            var updateCatalog =await _catalogService.Update(catalogEdit.Id, catalogEdit);
            if(updateCatalog != null) 
            {
                ViewBag.Message = Messages.CATALOG_EDIT_SUCCESSFUL;
                CatalogVM editedCatalog = _mapper.Map<Catalog, CatalogVM>(updateCatalog);
                return View(editedCatalog);
            }
            return View(catalogEdit);
        }
    }
}
