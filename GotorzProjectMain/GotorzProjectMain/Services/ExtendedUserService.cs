using GotorzProjectMain.Data;
using GotorzProjectMain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace GotorzProjectMain.Services
{

    public class ExtendedUserService : IExtendedUserService
    {
        private readonly ApplicationDbContext _context;

        public ExtendedUserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
        }

        // Load customers with their users
        public async Task<List<Customer>> GetCustomersWithUsersAsync()
        {
            return await _context.Customers
                                .Include(c => c.User)
                                .ToListAsync();
        }

        // Load employees with their users
        public async Task<List<Employee>> GetEmployeesWithUsersAsync()
        {
            return await _context.Employees
                                .Include(e => e.User)
                                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(string id)
        {
            return await _context.Customers
                                .Include(c => c.User)
                                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(string id)
        {
            return await _context.Employees
                                .Include(c => c.User)
                                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // (Optional) You could add more helper methods here, like:
        // public async Task<User> GetUserByIdAsync(string userId) { ... }
    }
}
