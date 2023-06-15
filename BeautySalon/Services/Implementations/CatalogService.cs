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

        public async Task<List<Catalog>> GetAll()
        {
            var list = await _dbContext.Catalogs.ToListAsync();

            return list;
        }
        
        public async Task<Catalog> Update(int catalogId, CatalogVM catalog)
        {
            var entity =await GetById(catalogId);
            if(entity!=null)
            {
                _mapper.Map(catalog, entity);
                _dbContext.Catalogs.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            return entity;
        }

        public async Task<Catalog> GetById(int catalogId)
        {
            var catalog = _dbContext.Catalogs.FirstOrDefault(c=>c.Id==catalogId);
            return catalog;
        }

        public async Task<List<Catalog>> SearchByName(string catalogName)
        {
            var catalogs = await _dbContext.Catalogs.Where(c => c.Title.ToLower().Contains(catalogName.ToLower()) 
            || string.IsNullOrEmpty(catalogName)).ToListAsync();
            return catalogs;
        }
    }
}
