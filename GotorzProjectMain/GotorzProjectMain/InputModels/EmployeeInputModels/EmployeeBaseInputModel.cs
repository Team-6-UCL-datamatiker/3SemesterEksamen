using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.InputModels.EmployeeInputModels
{
    public class EmployeeBaseInputModel
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
        [Phone]
        [Display(Name = "Phone")]
        public string Phone { get; set; } = "";

        [Display(Name = "Is Admin?")]
        public bool IsAdmin { get; set; }

        public IBrowserFile? ProfilePictureFile { get; set; }
    }
}
