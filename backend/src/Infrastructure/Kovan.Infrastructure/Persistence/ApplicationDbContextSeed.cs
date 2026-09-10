using Kovan.Domain.Constants;
using Kovan.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Kovan.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Varsayılan rolleri oluştur
        var adminRole = new IdentityRole(Roles.Admin);
        var userRole = new IdentityRole(Roles.User);

        if (!await roleManager.RoleExistsAsync(adminRole.Name!))
        {
            await roleManager.CreateAsync(adminRole);
        }

        if (!await roleManager.RoleExistsAsync(userRole.Name!))
        {
            await roleManager.CreateAsync(userRole);
        }

        // Varsayılan admin kullanıcısını oluştur
        var adminUser = new ApplicationUser { UserName = "admin@kovan.com", Email = "admin@kovan.com", FirstName = "Admin", LastName = "User" };

        if (userManager.Users.All(u => u.UserName != adminUser.UserName))
        {
            await userManager.CreateAsync(adminUser, "Admin123!");
            await userManager.AddToRoleAsync(adminUser, adminRole.Name!);
        }
    }
}