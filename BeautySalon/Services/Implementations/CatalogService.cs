using AutoMapper;
using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Helper;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace BeautySalon.Services.Implementations
{
    public class CatalogService : ICatalogService
    {
        protected ApplicationDbContext _dbContext;
        protected IMapper _mapper { get; set; }
        
        public CatalogService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<Catalog>> GetAllCatalogs()
        {
            var list = await _dbContext.Catalogs.ToListAsync();
            return list;
        }

        public async Task<List<CatalogVM>> GetAll()
        {
            var list = await _dbContext.Catalogs.ToListAsync();
            var catalogs = _mapper.Map<List<CatalogVM>>(list);
            return catalogs;
        }
        
        public async Task<CatalogVM> Update(int catalogId, CatalogVM catalog)
        {
            Catalog catalogToEdit = _dbContext.Catalogs.FindAsync(catalogId);
            if(catalog!=null)
            {
                catalog1.Update(catalog1);
                await _dbContext.SaveChangesAsync();
            }
            return catalog;
        }

        public async Task<CatalogVM> GetById(int catalogId)
        {
            var catalog = _dbContext.Catalogs.FirstOrDefault(c=>c.Id==catalogId);
            CatalogVM entity = _mapper.Map<CatalogVM>(catalog);
            return entity;
        }

        public async Task<List<Catalog>> SearchByName(string catalogName)
        {
            var catalogs = await _dbContext.Catalogs.Where(c => c.Title.ToLower().Contains(catalogName.ToLower()) 
            || string.IsNullOrEmpty(catalogName)).ToListAsync();
            return catalogs;
        }
    }
}
