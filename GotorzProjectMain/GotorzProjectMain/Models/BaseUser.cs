using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.Models;

public abstract class BaseUser
{
    [Key]
    [ForeignKey("User")]
    public string Id { get; set; }

    public ApplicationUser User { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = new byte[8];
}
