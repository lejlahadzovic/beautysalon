using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Models;
using BeautySalon.Services.Implementations;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Common;

namespace BeautySalon.Controllers
{

    public class CatalogController : Controller
    {
        protected new readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index()
        {
            var catalogs = await _catalogService.GetCatalogs(null);
            return View(catalogs);
        }
    }
}
