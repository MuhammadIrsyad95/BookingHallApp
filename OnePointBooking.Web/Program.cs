using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnePointBooking.Application.Common.Interfaces;
using OnePointBooking.Application.Services.Implementation;
using OnePointBooking.Application.Services.Interface;
using OnePointBooking.Domain.Entities;
using OnePointBooking.Infrastructure.Data;
using OnePointBooking.Infrastructure.Respository;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddDbContext<ApplicationDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("OnePointBooking")));

// Add Identity services with token providers
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Configure token providers for password reset, two-factor authentication, etc.
    options.Tokens.PasswordResetTokenProvider = "Default"; // Add token provider for password reset
    options.Tokens.ChangeEmailTokenProvider = "Default";  // You can add more if needed
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders(); // Ensure this is added for default token providers like password reset

builder.Services.ConfigureApplicationCookie(option =>
{
    option.AccessDeniedPath = "/Account/AccessDenied";
    option.LoginPath = "/Account/Login";
});

builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequiredLength = 6;
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomPackageService, RoomPackageService>();
builder.Services.AddScoped<IAmenityService, AmenityService>();
builder.Services.AddScoped<IRoomSetupService, RoomSetupService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
SeedDatabase();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
