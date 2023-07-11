using BeautySalon.Contracts;
using BeautySalon.Models;

namespace BeautySalon.Services.Interfaces
{
    public interface IAppointmentService
    {
      Task<List<AppointmentVM>> GetAppointmentsByUser(User user);
      Task<AppointmentVM> Create(User user, DateTime dateTime, int serviceId);
      Task<Appointment> Cancel(int id);
    }
}
