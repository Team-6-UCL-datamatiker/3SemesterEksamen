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

        // Load all customers with their users
        public async Task<List<Customer>> GetCustomersWithUsersAsync()
        {
            return await _context.Customers
                                .Include(c => c.User)
                                .ToListAsync();
        }

        // Load all employees with their users
        public async Task<List<Employee>> GetEmployeesWithUsersAsync()
        {
            return await _context.Employees
                                .Include(e => e.User)
                                .ToListAsync();
        }

		// Returns a specific customer by ID with user data
		public async Task<Customer?> GetCustomerByIdAsync(string id)
        {
            return await _context.Customers
                                .Include(c => c.User)
                                .FirstOrDefaultAsync(c => c.Id == id);
        }

		// Returns a specific employee by ID with user data
		public async Task<Employee?> GetEmployeeByIdAsync(string id)
        {
            return await _context.Employees
                                .Include(c => c.User)
                                .FirstOrDefaultAsync(c => c.Id == id);
        }

		// Returns a customer or employee (with user data) by ID
		public async Task<BaseUser?> GetCustomerOrEmployeeAsync(string id)
        {
            switch (_context.Employees.Any(c => c.Id == id))
            {
                case true:
                    return await _context.Employees
                                    .Include(c => c.User)
                                    .FirstOrDefaultAsync(c => c.Id == id);

                case false:
                    return await _context.Customers
                                    .Include(c => c.User)
                                    .FirstOrDefaultAsync(c => c.Id == id);
            }
        }
    }
}
