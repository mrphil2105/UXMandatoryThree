using CafeAnalog;
using CafeAnalog.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services.AddDbContext<AppDbContext>(o => o.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
services.AddDatabaseDeveloperPageExceptionFilter();

services.AddDefaultIdentity<AppUser>(o =>
    {
        o.Password.RequiredLength = 8;
        o.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AppDbContext>();

var mvcBuilder = services.AddControllersWithViews();

if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

#if DEBUG
services.AddHostedService(p => new NpmWatchHostedService(p.GetRequiredService<IWebHostEnvironment>()
    .IsDevelopment(), p.GetRequiredService<ILogger<NpmWatchHostedService>>()));
#endif

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();
