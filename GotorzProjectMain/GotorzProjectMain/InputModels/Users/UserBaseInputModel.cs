using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.InputModels.Users;

public abstract class UserBaseInputModel
{
    [Required]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = "";

    [Required]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Phone]
    public string? Phone { get; set; }
}
