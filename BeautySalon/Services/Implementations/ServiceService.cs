using AutoMapper;
using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Helper;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BeautySalon.Services.Implementations
{
    public class ServiceService : IServiceService
    {
        protected ApplicationDbContext _dbContext;
        protected IMapper _mapper { get; set; }
        
        public ServiceService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CatalogServiceVM> GetServices(int catalogId, string name)
        {   
            Catalog catalog =await _dbContext.Catalogs.FindAsync(catalogId);
            List<Service> services =await _dbContext.Services.Where(s=>s.CatalogId == catalogId 
            && (string.IsNullOrEmpty(name) 
            || s.Name.ToLower().Contains(name.ToLower()))).ToListAsync();
            
            CatalogServiceVM catalogServiceVM = new CatalogServiceVM();
            catalogServiceVM.CatalogId = catalogId;
            catalogServiceVM.Services = _mapper.Map<List<ServiceVM>>(services);
            catalogServiceVM.Title = catalog.Title;
            
            return catalogServiceVM;
        }

        public async Task<Service> GetServiceById(int id)
        {
            var service = await _dbContext.Services.FindAsync(id);
            
            return service;
        }

        public async Task<List<ServiceVM>> Get(string name, int catalogId)
        {
            var list =new List<Service>();
            list=await _dbContext.Services.Where(s => (s.CatalogId == catalogId || catalogId == 0)
            && (string.IsNullOrEmpty(name) || s.Name.ToLower().Contains(name.ToLower()))).ToListAsync();
           
            return _mapper.Map<List<ServiceVM>>(list);
        }

        public async Task<Service> Insert(ServiceVM insert)
        {
            var set = _dbContext.Services;
            Service entity = _mapper.Map<Service>(insert);
            entity.Catalog = _dbContext.Catalogs.Where(x => x.Id.Equals(insert.CatalogId)).First();
            set.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Service> Update(ServiceVM update)
        {
            var entity = await GetServiceById(update.Id);
            if (entity != null)
            {
                if (update.CatalogId != 0) {
                    update.Catalog =_dbContext.Catalogs.Where(x => x.Id.Equals(update.CatalogId)).First();
                }
                _mapper.Map(update, entity);
                _dbContext.Services.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
           
            return entity;
        }

        public async Task<Service> Delete(int id)
        {
            var entity = _dbContext.Services.FirstOrDefault(x => x.Id == id);
            if (entity != null) {
                _dbContext.Services.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }

            return entity;
        }
    }
}
