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

        public async Task<List<CatalogVM>> GetAll()
        {
            var list = await _dbContext.Catalogs.ToListAsync();

            return _mapper.Map<List<CatalogVM>>(list);
        }
    }
}
