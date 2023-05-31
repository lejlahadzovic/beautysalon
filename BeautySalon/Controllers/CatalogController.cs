using BeautySalon.Context;
using BeautySalon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Common;

namespace BeautySalon.Controllers
{
   
    public class CatalogController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CatalogController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View(_dbContext.Catalogs);
        }
    }
}
