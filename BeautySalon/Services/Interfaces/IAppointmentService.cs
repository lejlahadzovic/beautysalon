using BeautySalon.Contracts;
using BeautySalon.Models;

namespace BeautySalon.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<AppointmentVM>> GetAppointments(int userId = 0, int catalogId = 0, int serviceId = 0, bool isApproved=false);
        Task<List<AppointmentVM>> GetApprovedAppointments();
        Task<Appointment> GetById(int id);
    }
}
