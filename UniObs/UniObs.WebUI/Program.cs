using Microsoft.EntityFrameworkCore;
using UniObs.Infrastructure.Persistence;
using UniObs.Infrastructure.Persistence.Repositories;
using UniObs.Infrastructure.Services;
using UniObs.Application.Contracts.Repositories;
using UniObs.Application.Contracts.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<ObsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// Dependency Injection
builder.Services.AddScoped<IYoneticiRepository, YoneticiRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// 🔥 Yeni: Mail servisi
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
