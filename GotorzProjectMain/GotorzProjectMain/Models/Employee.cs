using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.Models
{
	public class Employee
	{
		[Key]
		[ForeignKey("User")]
		public string Id { get; set; }

		public Uri? ProfilePicture { get; set; }
		public bool Role { get; set; }

		public ApplicationUser User { get; set; }

		[Timestamp]
		public byte[] RowVersion { get; set; } = new byte[8];
	}

}
