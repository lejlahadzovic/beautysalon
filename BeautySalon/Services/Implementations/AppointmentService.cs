﻿using AutoMapper;
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

        public async Task<List<AppointmentVM>> GetAppointmentsByUser(int userId)
        {
            var appointments = await _dbContext.Appointments.Include(x=>x.Service).Where(x=>x.UserId.Equals(userId)).ToListAsync();

            return _mapper.Map<List<AppointmentVM>>(appointments);
        }

        public async Task<List<AppointmentVM>> SearchAppointments(int userId, AppointmentSearchVM search)
        {
            var appointments = await GetAppointmentsByUser(userId);
            if(appointments != null)
            {
                appointments = appointments.Where(x => x.ServiceId == search.ServiceId || search.ServiceId == 0
                && (x.Service.CatalogId == search.CatalogId || search.CatalogId == 0)
                && (x.Approved == search.IsApproved || x.Approved != search.IsUnapproved)).ToList();

                if (search.IsFutureAppointment && !search.IsPastAppointment) 
                {
                    appointments = appointments.Where(x => DateTime.Compare(x.StartDateTime,DateTime.Now) > 0).ToList();
                }
                else if(search.IsPastAppointment && !search.IsFutureAppointment)
                {
                    appointments = appointments.Where(x => DateTime.Compare(x.StartDateTime, DateTime.Now) < 0).ToList();
                }
            }

            return _mapper.Map<List<AppointmentVM>>(appointments);
        }

        public int CountAppointments(DateTime dateTime, int serviceId)
        {
            var appointments = _dbContext.Appointments.Where(x => x.ServiceId == serviceId && (x.StartDateTime.Equals(dateTime)
            || (DateTime.Compare(x.StartDateTime, dateTime) < 0 && DateTime.Compare(x.FinishDateTime, dateTime) > 0)
            || (DateTime.Compare(x.StartDateTime, dateTime.AddMinutes(x.Service.Duration)) < 0
            && DateTime.Compare(x.FinishDateTime, dateTime.AddMinutes(x.Service.Duration)) > 0))).Count();

            return appointments;
        }

        public async Task<AppointmentVM> Create(int userId,DateTime dateTime,int serviceId)
        {
            var service=_dbContext.Services.Where(x=>x.Id== serviceId).FirstOrDefault();
            var user = _dbContext.Users.Where(x => x.Id == userId).FirstOrDefault();
            int appointments = CountAppointments(dateTime, serviceId);
            if (appointments == 0)
            {
                Appointment entity = new Appointment();
                entity.UserId = userId;
                entity.User = user;
                entity.ServiceId = serviceId;
                entity.Service = service;
                entity.StartDateTime = dateTime;
                entity.FinishDateTime = dateTime.AddMinutes(service.Duration);
                entity.Approved = false;
                entity.Canceled = false;
                _dbContext.Appointments.Add(entity);
                _dbContext.SaveChanges();

                return _mapper.Map<AppointmentVM>(entity);
            }

            return null;
        }

        public async Task<Appointment> Cancel(int id)
        {
            var entity = _dbContext.Appointments.FirstOrDefault(x => x.Id == id && x.Canceled!=true);
            if (entity != null)
            {
                entity.Canceled = true;
                _dbContext.SaveChanges();
            }
             
            return entity;
        }
    }
}