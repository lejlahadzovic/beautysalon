using BeautySalon.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Catalog> Catalogs { get; set; }

   
    }
}
