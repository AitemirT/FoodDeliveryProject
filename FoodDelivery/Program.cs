using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.Models;
using FoodDelivery.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RestaurantSystemBdContext>(options => options.UseNpgsql(connection))
    .AddIdentity<MyUser, IdentityRole<int>>(options =>
    {

        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;


        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<RestaurantSystemBdContext>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

builder.Services.AddSingleton<EmailService>();

var app = builder.Build();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var userManager = services.GetRequiredService<UserManager<MyUser>>();
    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
    
    await AdminInitializer.SeedRolesAndAdmin(rolesManager, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while seeding the database.");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Restaurant}/{action=Index}/{id?}");

app.Run();