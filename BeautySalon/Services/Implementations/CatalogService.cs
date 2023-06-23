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
        
        public CatalogService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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

        public async Task<Catalog> Insert(CatalogVM insert)
        {
            var set = _dbContext.Catalogs;
            Catalog entity = _mapper.Map<Catalog>(insert);
            set.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        
        public async Task<Catalog> Update(int catalogId, CatalogVM update)
        {
            var entity = await GetById(catalogId);
            if(entity!=null)
            {
                _mapper.Map(update, entity);
                _dbContext.Catalogs.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            return entity;
        }

        public async Task Remove(Catalog remove)
        {
            _dbContext.Catalogs.Remove(remove);
            await _dbContext.SaveChangesAsync();
        }
    }
}
