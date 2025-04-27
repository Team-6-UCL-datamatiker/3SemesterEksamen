using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.InputModels.Users;

public abstract class UserBaseInputModel
{
    [Required]
    [Display(Name = "FirstName")]
    public string FirstName { get; set; } = "";

    [Required]
    [Display(Name = "LastName")]
    public string LastName { get; set; } = "";

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = "";

    [Phone]
    [Display(Name = "Phone")]
    public string Phone { get; set; } = "";
}
