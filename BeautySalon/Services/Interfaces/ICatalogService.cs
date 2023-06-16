using BeautySalon.Contracts;
using BeautySalon.Models;

namespace BeautySalon.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<Catalog>> GetAllCatalogs();
        Task<List<CatalogVM>> GetAll();
        Task<CatalogVM> GetById(int catalogId);
        Task<CatalogVM> Update(int catalogId, CatalogVM update);
        Task<List<Catalog>> SearchByName(string catalogName);
    }
}
