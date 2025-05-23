using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.Models;

public class ApplicationUser : IdentityUser
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }

	[Timestamp]
	public byte[] RowVersion { get; set; } = new byte[8];
}

