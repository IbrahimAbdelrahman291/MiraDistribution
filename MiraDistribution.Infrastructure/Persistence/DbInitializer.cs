using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiraDistribution.Domain.Enums;
using MiraDistribution.Infrastructure.Identity;

namespace MiraDistribution.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var configuration = services.GetRequiredService<IConfiguration>();

        foreach (var role in Enum.GetNames<UserRole>())
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var adminPhone = configuration["AdminSeed:Phone"];
        var adminPassword = configuration["AdminSeed:Password"];

        if (string.IsNullOrWhiteSpace(adminPhone) || string.IsNullOrWhiteSpace(adminPassword))
            return;

        var existingAdmin = await userManager.FindByNameAsync(adminPhone);
        if (existingAdmin is not null)
            return;

        var admin = new ApplicationUser { UserName = adminPhone, PhoneNumber = adminPhone };
        var result = await userManager.CreateAsync(admin, adminPassword);

        if (result.Succeeded)
            await userManager.AddToRoleAsync(admin, UserRole.Admin.ToString());
    }
}