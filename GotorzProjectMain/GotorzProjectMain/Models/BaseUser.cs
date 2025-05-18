using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.Models;

public abstract class BaseUser
{
    public string Id { get; set; }

	public string? CustomUserName { get; set; }

	public ApplicationUser User { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = new byte[8];
}
