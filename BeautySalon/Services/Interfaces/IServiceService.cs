using BeautySalon.Contracts;
using BeautySalon.Models;

namespace BeautySalon.Services.Interfaces
{
    public interface IServiceService
    {
        Task<CatalogServiceVM> GetServices(int catalogId, string name);
        Task<List<ServiceVM>> GetAll();
    }
}
