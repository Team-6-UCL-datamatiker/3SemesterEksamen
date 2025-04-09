using GotorzProjectMain.Data;
using GotorzProjectMain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // Add this


namespace GotorzProjectMain.Services
{
    public class UserService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly UserManager<ApplicationUser> _userManager; // Add this


        public UserService(IDbContextFactory<ApplicationDbContext> dbFactory, UserManager<ApplicationUser> userManager)
        {
            _dbFactory = dbFactory;
            _userManager = userManager; // Add this
        }

        // Load customers with their users
        public async Task<List<Customer>> GetCustomersWithUsersAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Customers
                                .Include(c => c.User)
                                .ToListAsync();
        }

        // Load employees with their users
        public async Task<List<Employee>> GetEmployeesWithUsersAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Employees
                                .Include(e => e.User)
                                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(string id)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Customers
                                .Include(c => c.User)
                                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(string id)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Employees
                                .Include(c => c.User)
                                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                // Handle the error if needed
                throw new Exception($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        // (Optional) You could add more helper methods here, like:
        // public async Task<User> GetUserByIdAsync(string userId) { ... }
    }
}
