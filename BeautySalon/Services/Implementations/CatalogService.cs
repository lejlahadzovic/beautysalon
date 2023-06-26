using AutoMapper;
using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Helper;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections;
using System.Runtime.CompilerServices;

namespace BeautySalon.Services.Implementations
{
    public class CatalogService : ICatalogService
    {
        protected ApplicationDbContext _dbContext;
        protected IMapper _mapper { get; set; }
        protected readonly IWebHostEnvironment _hostEnvironment;

        public CatalogService(ApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<List<CatalogVM>> GetAll()
        {
            var list = new List<Catalog>();
            list = await _dbContext.Catalogs.ToListAsync();
            return _mapper.Map<List<CatalogVM>>(list);
        }
        
        public async Task<List<CatalogVM>> SearchByName(string catalogName)
        {
            var catalogs = await _dbContext.Catalogs.Where(c => c.Title.ToLower().Contains(catalogName.ToLower()) 
            || string.IsNullOrEmpty(catalogName)).ToListAsync();
            var catalogMap = _mapper.Map<List<CatalogVM>>(catalogs);
            return catalogMap;
        }
        
        public async Task<Catalog> GetById(int catalogId)
        {
            var catalog = await _dbContext.Catalogs.FindAsync(catalogId);
            return catalog;
        }

        public async Task<Catalog> Insert(CatalogVM insert, IFormFile imgfile)
        {
            if (imgfile != null && imgfile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string imageFileName = $"{Path.GetFileNameWithoutExtension(imgfile.FileName)}{insert.Title}" +
                   $"{Path.GetExtension(imgfile.FileName)}";
                string filePath = Path.Combine(uploadsFolder, imageFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imgfile.CopyToAsync(fileStream);
                }
                insert.ImageFileString = imageFileName;
            }
            var set = _dbContext.Catalogs;
            Catalog entity = _mapper.Map<Catalog>(insert);
            set.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        
        public async Task<Catalog> Update(int catalogId, CatalogVM update, IFormFile imgfile)
        {
            var entity = await GetById(catalogId);
            if (imgfile != null && imgfile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string imageFileName = $"{Path.GetFileNameWithoutExtension(imgfile.FileName)}{update.Title}" +
                    $"{Path.GetExtension(imgfile.FileName)}";
                var imgFileFromExistingCatalog = entity.ImageFileString;
                if (imgFileFromExistingCatalog == imageFileName)
                {
                    var fullPath = uploadsFolder + "\\" + imageFileName;
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                else
                {
                    var existingImage = uploadsFolder + "\\" + entity.ImageFileString;
                    if (System.IO.File.Exists(existingImage))
                    {
                        System.IO.File.Delete(existingImage);
                    }
                }
                string filePath = Path.Combine(uploadsFolder, imageFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imgfile.CopyToAsync(fileStream);
                }
                update.ImageFileString = imageFileName;
            }
            if (entity!=null)
            {
                _mapper.Map(update, entity);
                _dbContext.Catalogs.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            return entity;
        }

        public async Task Remove(Catalog remove)
        {
            string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
            string imagesToDelete = $"*{remove.Title}*";
            string[] imagesList = System.IO.Directory.GetFiles(uploadFolder, imagesToDelete);
            foreach (string image in imagesList)
            {
                System.IO.File.Delete(image);
            }
            _dbContext.Catalogs.Remove(remove);
            await _dbContext.SaveChangesAsync();
        }
    }
}
