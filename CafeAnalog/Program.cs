using System.Text.Json;
using CafeAnalog;
using CafeAnalog.Data;
using CafeAnalog.Models;
using Microsoft.AspNetCore.Identity;
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
services.ConfigureApplicationCookie(o => o.LoginPath = "/Account/Login");

services.AddAntiforgery(o => o.HeaderName = "X-XSRF-TOKEN");

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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();

    if (!dbContext.ShopItems.Any())
    {
        await using var fileStream = File.OpenRead("shop-categories.json");
        var categories = JsonSerializer.Deserialize<List<ShopCategory>>(fileStream);

        dbContext.ShopCategories.AddRange(categories!);
        dbContext.SaveChanges();
    }

    if (!dbContext.Users.Any())
    {
        await using var fileStream = File.OpenRead("dummy-users.json");
        var rankings = JsonSerializer.Deserialize<List<RankingModel>>(fileStream);
        var users = rankings!.Select(r =>
            {
                string email = StringHelper.GenerateMail();

                return new AppUser
                {
                    FirstName = r.FirstName!,
                    LastName = r.LastName!,
                    UserName = email,
                    Email = email,
                    Score = r.Score
                };
            })
            .ToList();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        foreach (var user in users)
        {
            await userManager.CreateAsync(user);
        }
    }
}

await app.RunAsync();
