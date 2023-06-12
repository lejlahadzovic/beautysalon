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
    }
}
