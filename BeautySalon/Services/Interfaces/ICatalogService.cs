using BeautySalon.Contracts;
using BeautySalon.Models;

namespace BeautySalon.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<Catalog>> GetAll();
        Task<Catalog> GetById(int catalogId);
        Task<Catalog> Update(int catalogId, CatalogVM update);
        Task<List<Catalog>> SearchByName(string catalogName);
    }
}
