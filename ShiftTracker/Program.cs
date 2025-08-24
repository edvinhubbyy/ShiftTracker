using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ShiftTracker.Data;
using ShiftTracker.Data.Models;
using ShiftTracker.Seeders;
using ShiftTracker.Services.Core;
using ShiftTracker.Services.Core.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Get port from environment or default
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

// Set URLs before building the app
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls($"https://localhost:{port}");
}
else
{
    builder.WebHost.UseUrls($"http://*:{port}");
}

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure DbContext
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ShiftContext>(options =>
    options.UseSqlServer(connectionString));

// Add Authentication (Cookie-based)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";     // redirect if not logged in
        options.AccessDeniedPath = "/Auth/AccessDenied"; // redirect if no role
    });


// Add Authorization policies (optional, for convenience)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireManager", policy => policy.RequireRole("Manager", "Admin"));
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
});

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register application services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IShiftService, ShiftService>();

var app = builder.Build();

// Run seeders
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ShiftContext>();
    RoleSeeder.SeedAll(context);
}



// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication(); 
app.UseAuthorization();

// Map default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();