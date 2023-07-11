using AutoMapper;
using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        public async Task<List<AppointmentVM>> GetAppointmentsByUser(User user)
        {
            var appointments = await _dbContext.Appointments.Include(x=>x.Service).Where(x=>x.UserId.Equals(user.Id)).ToListAsync();

            return _mapper.Map<List<AppointmentVM>>(appointments);
        }

        public async Task<AppointmentVM> Create(User user,DateTime dateTime,int serviceId)
        {
            var appointments = await _dbContext.Appointments.Include(x => x.Service).Where(x => x.UserId.Equals(user.Id) && x.ServiceId==serviceId && x.StartDateTime.Date.Equals(dateTime.Date)).ToListAsync();
            if (appointments.Count == 0)
            {
                Appointment entity = new Appointment();
                entity.UserId = user.Id;
                entity.ServiceId = serviceId;
                entity.StartDateTime = dateTime;
                entity.FinishDateTime = dateTime;
                entity.Approved = false;
                entity.Service = _dbContext.Services.Where(x => x.Id.Equals(serviceId)).First();
                _dbContext.Appointments.Add(entity);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<AppointmentVM>(entity);
            }

            return null;
        }

        public async Task<Appointment> Cancel(int id)
        {
            var entity = _dbContext.Appointments.FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                entity.Canceled = true;
                _dbContext.SaveChanges();
            }

            return entity;
        }
    }
}
