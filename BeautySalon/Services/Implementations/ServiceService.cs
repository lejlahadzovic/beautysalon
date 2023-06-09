using AutoMapper;
using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<CatalogServiceVM> GetServicesByCatalogId(int catalogId)
        {   
            List<Service> services =await _dbContext.Services.Where(s=>s.CatalogId == catalogId).ToListAsync();
            Catalog catalog =await _dbContext.Catalogs.FindAsync(catalogId);

            CatalogServiceVM catalogServiceVM = new CatalogServiceVM();
            catalogServiceVM.Title = catalog.Title;
            catalogServiceVM.Services = _mapper.Map<List<Service>>(services);
            catalogServiceVM.CatalogId = catalogId;

            return catalogServiceVM;
            
        }
        public async Task<CatalogServiceVM> GetServicesByName(string name, int catalogId)
        {
            List<Service> services = await _dbContext.Services.Where(s => s.CatalogId==catalogId).ToListAsync();
            List<Service> servicesByName = services.Where(s => s.Name.ToLower().Contains(name.ToLower())).ToList();
            Catalog catalog = await _dbContext.Catalogs.FindAsync(catalogId);

            CatalogServiceVM catalogServiceVM = new CatalogServiceVM();
            catalogServiceVM.Title = catalog.Title;
            catalogServiceVM.Services = _mapper.Map<List<Service>>(services);

            return catalogServiceVM;
        }
    }
}
