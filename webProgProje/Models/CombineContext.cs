using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace webProgProje.Models
{
    public class CombineContext:DbContext
    {
        public DbSet<Kullanici> Kullanicilar {  get; set; } 
        public DbSet<Doktor> Doktorlar { get; set; }
        public DbSet<Hasta> Hastalar { get; set; }
        public DbSet<Admin> Adminler { get; }
        public DbSet<Anadal> Anadallar{ get; set;}
        public DbSet<Randevu> Randevular{ get; set;}
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\MSSQLSERVER02;Database=master;Trusted_Connection=True;TrustServerCertificate=True");
        }

    }
}
