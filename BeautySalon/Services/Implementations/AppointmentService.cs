using AutoMapper;
using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BeautySalon.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        protected ApplicationDbContext _dbContext;
        protected IMapper _mapper { get; set; }

        public AppointmentService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<AppointmentVM>> GetAppointments(int userId, int catalogId, int serviceId, bool isApproved)
        {
            var appointments = await _dbContext.Appointments
                .Include(u => u.User)
                .Include(s => s.Service)
                .Include(c => c.Service.Catalog)
                .Where(a =>
                    (userId == 0 || a.UserId == userId)
                    && (catalogId == 0 || a.Service.CatalogId == catalogId)
                    && (serviceId == 0 || a.ServiceId == serviceId)
                    && (a.Approved==isApproved || isApproved==false))
                .ToListAsync();

            return _mapper.Map<List<AppointmentVM>>(appointments);
        }

        public async Task<List<AppointmentVM>> GetApprovedAppointments()
        {
            var appointments = await _dbContext.Appointments.Where(x => x.Approved == true).ToListAsync();
            return _mapper.Map<List<AppointmentVM>>(appointments);
        }

        public async Task<Appointment> GetById(int appointmentId)
        {
            var appointment = await _dbContext.Appointments.FindAsync(appointmentId);
            return appointment;
        }
    }
}
