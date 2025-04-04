namespace GotorzProjekt.Models
{
	public class Employee : User
	{
		public Uri ProfilePicture { get; set; }
		public bool Role { get; set; }
	}
}
