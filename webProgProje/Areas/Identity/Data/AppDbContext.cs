using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webProgProje.Areas.Identity.Data;
using webProgProje.Models;

namespace webProgProje.Data;

//kullanılmıyor
public class AppDbContext : IdentityDbContext<DbKullanici>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


}
