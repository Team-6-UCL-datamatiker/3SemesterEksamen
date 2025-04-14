using GotorzProjectMain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GotorzProjectMain.Data;

public static class DbInitializer
{
    public static async Task Execute(IServiceProvider serviceProvider, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        // Ensure the database is created and apply any pending migrations
        await context.Database.MigrateAsync();

        await CreateUserRolesIfNotExists(serviceProvider);

        await CreateAdminIfNotExists(context, userManager);
    }

    private static async Task CreateUserRolesIfNotExists(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { "Admin", "Employee", "Customer" };

        foreach (var roleName in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));

                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create role '" + roleName + "': " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }

    private static async Task CreateAdminIfNotExists(ApplicationDbContext context, UserManager<ApplicationUser> manager)
    {
        bool adminExists = context.Employees.Any(e => e.Role == true);

        if (!adminExists)
        {
            var user = new ApplicationUser
            {
                UserName = "admin@admin",
                Email = "admin@admin",
                EmailConfirmed = true
            };

            var result = await manager.CreateAsync(user, "@Dmin1");

            if (!result.Succeeded)
            {
                throw new Exception("Failed to create admin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var employee = new Employee
            {
                Role = true,
                User = user
            };

            context.Employees.Add(employee);
            await context.SaveChangesAsync();
        }
    }
}