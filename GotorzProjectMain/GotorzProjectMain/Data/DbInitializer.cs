using GotorzProjectMain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GotorzProjectMain.Data;

public static class DbInitializer
{
	// Entry point for database setup
	public static async Task Execute(IServiceProvider serviceProvider, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        // Ensure the database is created and apply any pending migrations
        await context.Database.MigrateAsync();

		// Ensure roles exist
		await CreateUserRolesIfNotExists(serviceProvider);

		// Ensure admin user exists
		await CreateAdminIfNotExists(context, userManager);
    }

	// Creates roles if they haven't been set up yet
	private static async Task CreateUserRolesIfNotExists(IServiceProvider serviceProvider)
    {
		// Resolve the RoleManager service from the DI container
		var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { "Admin", "Employee", "Customer" };

        foreach (var roleName in roleNames)
        {
			// Check if the role already exists
			var roleExists = await roleManager.RoleExistsAsync(roleName);

			// If not, create it
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

	// Creates a default admin user if none exists
	private static async Task CreateAdminIfNotExists(ApplicationDbContext context, UserManager<ApplicationUser> manager)
    {
		// Check if any admin already exists

		bool adminExists = context.Employees.Any(e => e.IsAdmin == true);

        if (!adminExists)
        {
			// Create a new ApplicationUser representing the admin
			var user = new ApplicationUser
            {
                UserName = "admin@admin",
                Email = "admin@admin",
                EmailConfirmed = true
            };

			// Attempt to create the user with a default password
			var result = await manager.CreateAsync(user, "@Dmin1");

			// If user creation fails, throw with detailed error messages
			if (!result.Succeeded)
            {
                throw new Exception("Failed to create admin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

			// Assign the user to the Admin role
			var roleResult = await manager.AddToRoleAsync(user, "Admin");

			if (!roleResult.Succeeded)
			{
				throw new Exception("Failed to assign Admin role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
			}

			// Register the admin as an Employee (linking to Identity user)
			var employee = new Employee
            {
                IsAdmin = true,
                User = user,
                CustomUserName = "admin",
                ProfilePicture = "images/admindefault.jpg"
            };

            context.Employees.Add(employee);
            await context.SaveChangesAsync();
        }
    }
}