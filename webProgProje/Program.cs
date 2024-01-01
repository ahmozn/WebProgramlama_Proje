using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using webProgProje.Data;
using webProgProje.Models;
using webProgProje.Areas.Identity.Data;
using Microsoft.Build.Execution;
using System.Globalization;
using webProgProje.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DbContextConnection") ?? throw new InvalidOperationException("Connection string 'DbContextConnection' not found.");

builder.Services.AddDbContext<CombineContext>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddIdentity<Kullanici, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
})
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<CombineContext>();

builder.Services.AddControllersWithViews()
    .AddViewLocalization();
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new("tr-TR");

    CultureInfo[] cultures = new CultureInfo[]
    {
        new("tr-TR"),
        new("en-US")
    };

    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});
builder.Services.AddScoped<RequestLocalizationCookiesMiddleware>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(45);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();;
app.UseAuthorization();

app.UseRequestLocalization();
app.UseRequestLocalization();
app.UseRequestLocalizationCookies();

builder.Services.AddRazorPages();
app.UseEndpoints(endpoints =>endpoints.MapRazorPages());
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Anasayfa}/{id?}");

app.Run();
