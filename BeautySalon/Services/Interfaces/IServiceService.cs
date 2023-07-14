using BeautySalon.Contracts;
using BeautySalon.Models;
using System.Threading.Tasks;

namespace BeautySalon.Services.Interfaces
{
    public interface IServiceService
    {
        Task<CatalogServiceVM> GetServices(int catalogId, string name);
        Task<List<ServiceVM>> Get(int catalogId, string name = "");
        Task<Service> GetServiceById(int id);
        Task<Service> Insert(ServiceVM insert);
        Task<Service> Update(ServiceVM update);
        Task<Service> Delete(int id);
        Task<int> CheckAppointment(int id);
    }
}
