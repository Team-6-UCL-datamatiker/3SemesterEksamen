using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.Models;

public abstract class BaseUser
{
    public string Id { get; set; }

	public string? CustomUserName { get; set; }

	public ApplicationUser User { get; set; }

	// Helps prevent data conflicts — if someone else has changed this user since you loaded it, saving will fail with a concurrency error
	[Timestamp]
    public byte[] RowVersion { get; set; } = new byte[8];
}
