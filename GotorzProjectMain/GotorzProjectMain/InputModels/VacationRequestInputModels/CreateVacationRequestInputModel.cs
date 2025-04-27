using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.InputModels.VacationRequestInputModels
{
	public class CreateVacationRequestInputModel : VacationRequestBaseInputModel
	{
		[Display(Name = "Customer Email")]
		[EmailAddress]
		public string? CustomerEmail { get; set; }
	}
}
