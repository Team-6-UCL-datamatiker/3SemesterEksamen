using GotorzProjectMain.Models;

namespace GotorzProjectMain.Services
{
	public interface IExtendedUserService
	{
		Task<List<Customer>> GetCustomersWithUsersAsync();
		Task<List<Employee>> GetEmployeesWithUsersAsync();
		Task<Customer?> GetCustomerByIdAsync(string id);
		Task<Employee?> GetEmployeeByIdAsync(string id);
	}
}
