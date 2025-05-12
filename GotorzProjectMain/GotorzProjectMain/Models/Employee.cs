namespace GotorzProjectMain.Models;

public class Employee : BaseUser
{
	public string? ProfilePicture { get; set; }
	public bool IsAdmin { get; set; }
}
