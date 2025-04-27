namespace GotorzProjectMain.Models;

public class Employee : BaseUser
{
	public Uri? ProfilePicture { get; set; }
	public bool IsAdmin { get; set; }
}
