using BeautySalon.Contracts;
using BeautySalon.Models;

namespace BeautySalon.Services.Interfaces
{
    public interface IAppointmentService
    {
      Task<List<AppointmentVM>> GetAppointmentsByUser(int userId);
      Task<AppointmentVM> Create(int userId, DateTime dateTime, int serviceId);
      Task<Appointment> Cancel(int id);
      Task<List<AppointmentVM>> SearchAppointments(int userId, AppointmentSearchVM search);
    }
}
