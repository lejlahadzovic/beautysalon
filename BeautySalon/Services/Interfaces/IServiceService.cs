using BeautySalon.Contracts;
using BeautySalon.Models;

namespace BeautySalon.Services.Interfaces
{
    public interface IServiceService
    {
        Task<CatalogServiceVM> GetServices(int catalogId, string name);
        Task<List<ServiceVM>> GetAll(string name);
        Task<Service> GetServicesById(int id);
        Task<Service> Insert(ServiceVM insert);
        Task<Service> Update(int id, ServiceVM update);
    }
}
