using BeautySalon.Contracts;

namespace BeautySalon.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<AppointmentVM>> GetAppointments();
    }
}
