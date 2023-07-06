using BeautySalon.Contracts;
using BeautySalon.Models;

namespace BeautySalon.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<Catalog> GetById(int catalogId);
        Task<Catalog> Insert(CatalogVM insert);
        Task<Catalog> Update(int id, CatalogVM update);
        Task Remove(Catalog remove);
        Task<List<CatalogVM>> GetCatalogs(string catalogName = "");
    }
}
