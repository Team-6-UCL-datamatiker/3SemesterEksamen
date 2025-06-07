using GotorzProjectMain.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GotorzProjectMain.Services
{


	public class CurrentUserService : ICurrentUserService
	{
		// Used to get info about the currently logged-in user
		private readonly AuthenticationStateProvider _authStateProvider;

		private readonly ApplicationDbContext _db;

		// The current authenticated user
		public ClaimsPrincipal? User { get; set; }


		public CurrentUserService(
		AuthenticationStateProvider authStateProvider,
		ApplicationDbContext db)
		{
			_authStateProvider = authStateProvider;
			_db = db;
		}

		// Gets the current user's ID (or null if not authenticated)
		public async Task<string?> GetUserIdAsync()
		{
			var authState = await _authStateProvider.GetAuthenticationStateAsync();
			User = authState.User;
			return User.FindFirstValue(ClaimTypes.NameIdentifier);
		}

		// Checks if the current user is registered as an employee
		public async Task<bool> IsEmployeeAsync()
		{
			string? userId = await GetUserIdAsync();
			if (userId == null) return false;

			// returns true if the user is an employee
			return await _db.Employees.AnyAsync(e => e.Id == userId);
		}
	}
}
