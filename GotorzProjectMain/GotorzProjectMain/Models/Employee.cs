using GotorzProjectMain.Data;

namespace GotorzProjectMain.Models
{
	public class Employee : ApplicationUser
	{
		public Uri? ProfilePicture { get; set; }
		public bool Role { get; set; }
	}
}
