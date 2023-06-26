using BeautySalon.Contracts;
using BeautySalon.Models;

namespace BeautySalon.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<CatalogVM>> GetAll();
        Task<Catalog> GetById(int catalogId);
        Task<Catalog> Insert(CatalogVM insert, IFormFile imgfile);
        Task<Catalog> Update(int id, CatalogVM update, IFormFile imgfile);
        Task Remove(Catalog remove);
        Task<List<CatalogVM>> SearchByName(string catalogName);
    }
}
