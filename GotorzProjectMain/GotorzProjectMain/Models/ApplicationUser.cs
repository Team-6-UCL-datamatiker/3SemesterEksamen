using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.Models;

public class ApplicationUser : IdentityUser
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }

	// Helps prevent data conflicts — if someone else has changed this user since you loaded it, saving will fail with a concurrency error
	[Timestamp]
	public byte[] RowVersion { get; set; } = new byte[8];
}

