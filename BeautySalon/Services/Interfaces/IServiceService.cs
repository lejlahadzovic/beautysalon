using BeautySalon.Contracts;
using BeautySalon.Models;

namespace BeautySalon.Services.Interfaces
{
    public interface IServiceService
    {
        Task<CatalogServiceVM> GetServices(int catalogId, string name);
        Task<List<ServiceVM>> Get(string name, int catalogId);
        Task<Service> GetServiceById(int id);
        Task<Service> Insert(ServiceVM insert, int catalogId);
        Task<Service> Update(int id, ServiceVM update,int catalogId);
        Task<Service> Delete(int id);
    }
}
