using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.Models.Forms
{
    public sealed class CustomerInput
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

        // Added to make sure a username is written
        [Required]
        [Display(Name = "Username")]
        public string CustomUserName { get; set; } = "";

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "";

        [Phone]
        [Display(Name = "Phone")]
        public string Phone { get; set; } = "";
    }
}
