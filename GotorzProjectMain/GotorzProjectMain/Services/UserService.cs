using GotorzProjectMain.Data;
using GotorzProjectMain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace GotorzProjectMain.Services
{
    public class UserService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;


        public UserService(IDbContextFactory<ApplicationDbContext> dbFactory, UserManager<ApplicationUser> userManager)
        {
            _dbFactory = dbFactory;
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

        // (Optional) You could add more helper methods here, like:
        // public async Task<User> GetUserByIdAsync(string userId) { ... }
    }
}
