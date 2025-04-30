using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.InputModels.Users.CustomerInputModels
{
    public class CustomerBaseInputModel : UserBaseInputModel
    {
        [Required]
        [Display(Name = "Username")]
        public string CustomUserName { get; set; } = "";
    }
}
