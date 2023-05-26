using AutoMapper;
using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Helper;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Services.Implementations
{
    public class BaseService<TModel, TDb, TInsert, TSearch> : IBaseService<TModel,TDb, TInsert,TSearch> where TModel : class where TInsert:class where TDb : class where TSearch : class
    {
        protected ApplicationDbContext _dbContext;
        protected IMapper _mapper { get; set; }
        public BaseService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public virtual async Task<TDb> Insert(TInsert insert)
        {
            var set = _dbContext.Set<TDb>();
            TDb entity = _mapper.Map<TDb>(insert);
            set.Add(entity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TDb>(entity);
        }

        public virtual async Task<TDb> GetByID(int id)
        {
            var entity = await _dbContext.Set<TDb>().FindAsync(id);

            return _mapper.Map<TDb>(entity);
        }

        public virtual async Task<TDb> GetAll()
        {
            
           var list =await _dbContext.Set<TDb>().ToListAsync();

           return _mapper.Map<TDb>(list);
        }
    }
}
