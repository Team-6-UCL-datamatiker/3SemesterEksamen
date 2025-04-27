using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.InputModels.Users.EmployeeInputModels
{
    public class EmployeeBaseInputModel : UserBaseInputModel
    {
        [Display(Name = "Is Admin?")]
        public bool IsAdmin { get; set; }

        public IBrowserFile? ProfilePictureFile { get; set; }
    }
}
