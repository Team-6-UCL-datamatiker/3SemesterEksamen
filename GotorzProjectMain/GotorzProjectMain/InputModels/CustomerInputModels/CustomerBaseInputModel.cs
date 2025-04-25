using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.InputModels.CustomerInputModels
{
    public class CustomerBaseInputModel
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

        [Required]
        [Display(Name = "Username")]
        public string CustomUserName { get; set; } = "";

        [Phone]
        [Display(Name = "Phone")]
        public string Phone { get; set; } = "";

    }
}
