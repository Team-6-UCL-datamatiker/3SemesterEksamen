using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }

	[Timestamp]
	public byte[] RowVersion { get; set; } = new byte[8];
}

