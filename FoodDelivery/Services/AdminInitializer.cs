using FoodDelivery.Models;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Services;

public class AdminInitializer
{
    public static async Task SeedRolesAndAdmin(RoleManager<IdentityRole<int>> roleManager, UserManager<MyUser> userManager)
    {
        string adminEmail = "admin@admin.com";
        string adminPassword = "1qwe@QWE";
        string adminUserName = "admin";

        var roles = new[] { "admin", "user"};
        
        foreach (var role in roles)
        {
            if (await roleManager.FindByNameAsync(role) is null)
                await roleManager.CreateAsync(new IdentityRole<int>(role));
        }

        if (await userManager.FindByNameAsync(adminEmail) == null)
        {
            MyUser admin = new MyUser { Email = adminEmail, UserName = adminUserName};
            IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "admin");
        }
    }
}