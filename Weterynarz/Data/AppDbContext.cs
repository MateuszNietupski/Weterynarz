
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Weterynarz.Models;

namespace Weterynarz.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Wizyta> Wizyty { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehaviour",true);

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
        }
        private static void SeedRoles(ModelBuilder modelBuilder)
        {

        }





    }
}
