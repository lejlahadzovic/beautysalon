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
        Task<List<AppointmentVM>> GetAppointments(int userId, int catalogId, int serviceId, bool isApproved, bool isCanceled,
            DateTime? dateFrom, DateTime? dateTo, string dateRange);
        Task Approve(Appointment appointment);
        Task<Appointment> GetById(int id);
    }
}
