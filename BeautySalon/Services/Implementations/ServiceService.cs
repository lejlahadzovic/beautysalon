﻿using AutoMapper;
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

        public async Task<Service> GetServicesById(int id)
        {
            var service = await _dbContext.Services.FindAsync(id);
            
            return service;
        }

        public List<Catalog> GetCatalogs()
        {
            var catalogs = _dbContext.Catalogs.ToList();

            return catalogs;
        }

        public async Task<List<ServiceVM>> GetAll(string name, int catalogId)
        {
            var list =new List<Service>();
            if (!string.IsNullOrEmpty(name))
            {
                list=await _dbContext.Services.Include(c => c.Catalog).Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            }
            else if (catalogId != 0)
            {
                list = await _dbContext.Services.Include(c => c.Catalog).Where(x => x.CatalogId.Equals(catalogId)).ToListAsync();
            }
            else
            {
                list=await _dbContext.Services.Include(c => c.Catalog).ToListAsync();
            }

            return _mapper.Map<List<ServiceVM>>(list);
        }

        public async Task<Service> Insert(ServiceVM insert, int catalogId)
        {
            var set = _dbContext.Services;
            Service entity = _mapper.Map<Service>(insert);
            entity.CatalogId = catalogId;
            entity.Catalog = _dbContext.Catalogs.Where(x => x.Id.Equals(catalogId)).First();
            set.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Service> Update(int id, ServiceVM update, int catalogId)
        {
            var entity = await GetServicesById(id);
            if (entity != null)
            {
                if (catalogId != 0) {
                    update.CatalogId = catalogId;
                    update.Catalog =_dbContext.Catalogs.Where(x => x.Id.Equals(catalogId)).First();
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
