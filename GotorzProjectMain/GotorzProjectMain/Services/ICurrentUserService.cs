using System.Security.Claims;

namespace GotorzProjectMain.Services
{
	public interface ICurrentUserService
	{
		Task<string?> GetUserIdAsync();
		Task<bool> IsEmployeeAsync();
		ClaimsPrincipal? User { get; }
	}
}
