using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using webProgProje.Areas.Identity.Data;

namespace webProgProje.Models
{
    public class CombineContext:IdentityDbContext<DbKullanici>
    {
        public DbSet<Kullanici> Kullanicilar {  get; set; } 
        public DbSet<Doktor> Doktorlar { get; set; }
        public DbSet<Hasta> Hastalar { get; set; }
        public DbSet<Admin> Adminler { get; set; }
        public DbSet<Anadal> Anadallar{ get; set;}
        public DbSet<Randevu> Randevular{ get; set;}
        
        public CombineContext(DbContextOptions<CombineContext>options) : base(options) { }
        public CombineContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\MSSQLSERVER02;Database=Proje;Trusted_Connection=True;TrustServerCertificate=True",x=>x.UseDateOnlyTimeOnly());
        }
    }
}
