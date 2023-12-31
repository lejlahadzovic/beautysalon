﻿using AutoMapper;
using BeautySalon.Context;
using BeautySalon.Contracts;
using BeautySalon.Helper;
using BeautySalon.Models;
using BeautySalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BeautySalon.Services.Implementations
{
    public class CatalogService : ICatalogService
    {
        protected ApplicationDbContext _dbContext;
        protected IMapper _mapper { get; set; }
        protected readonly IWebHostEnvironment _hostEnvironment;

        public CatalogService(ApplicationDbContext dbContext, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<List<CatalogVM>> GetCatalogs(string catalogName)
        {
            List<Catalog> catalogs = await _dbContext.Catalogs
                .Where(c=> string.IsNullOrWhiteSpace(catalogName) 
                || c.Title.ToLower().Contains(catalogName.ToLower()))
                .ToListAsync();
            return _mapper.Map<List<CatalogVM>>(catalogs);
        }

        public async Task<Catalog> GetById(int catalogId)
        {
            var catalog = await _dbContext.Catalogs.FindAsync(catalogId);
            return catalog;
        }

        public async Task<Catalog> Insert(CatalogVM insert)
        {
            if (insert.UploadFile != null)
            {
                insert.ImageFileString = UploadFile(insert.UploadFile);
            }
            var set = _dbContext.Catalogs;
            Catalog entity = _mapper.Map<Catalog>(insert);
            set.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Catalog> Update(int catalogId, CatalogVM update)
        {
            var entity = await GetById(catalogId);
            if (entity != null)
            {
                if (update.UploadFile != null)
                {
                    if (entity.ImageFileString != null)
                    {
                        string existingImageFile = Path.Combine(_hostEnvironment.WebRootPath, "images", entity.ImageFileString);
                        System.IO.File.Delete(existingImageFile);
                    }
                    update.ImageFileString = UploadFile(update.UploadFile);
                }
                else
                {
                    update.ImageFileString = entity.ImageFileString;
                }
                _mapper.Map(update, entity);
                _dbContext.Catalogs.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            return entity;
        }

        public async Task Remove(Catalog remove)
        {
            if (remove.ImageFileString != null)
            {
                string existingImageFile = Path.Combine(_hostEnvironment.WebRootPath, "images", remove.ImageFileString);
                System.IO.File.Delete(existingImageFile);
            }
            _dbContext.Catalogs.Remove(remove);
            await _dbContext.SaveChangesAsync();
        }

        private string UploadFile(IFormFile imgfile)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "-" + imgfile.FileName;
            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", uniqueFileName);
            using (var stream = new FileStream(uploadsFolder, FileMode.Create))
            {
                imgfile.CopyTo(stream);
            }
            return uniqueFileName;
        }

        public async Task<bool> CheckAppointments(int catalogId)
        {
            var serviceIds = await _dbContext.Services
                .Where(s => s.CatalogId == catalogId)
                .Select(x => x.Id)
                .ToListAsync();

            var appointments = await _dbContext.Appointments
                .Where(a => serviceIds.Contains(a.ServiceId)
                && a.StartDateTime >= DateTime.Today)
                .ToListAsync();

            if (appointments.Any())
            {
                return true;
            }
            return false;
        }
    }
}
