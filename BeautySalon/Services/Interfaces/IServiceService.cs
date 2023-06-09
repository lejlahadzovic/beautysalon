using BeautySalon.Contracts;
using BeautySalon.Models;

namespace BeautySalon.Services.Interfaces
{
    public interface IServiceService
    {
        Task<CatalogServiceVM> GetServicesByCatalogId(int catalogId);
        Task<CatalogServiceVM> GetServicesByName(string name, int catalogId);
    }
}
