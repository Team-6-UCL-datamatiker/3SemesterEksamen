using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.InputModels.VacationOfferInputModels
{
    public class VacationOfferBaseInputModel
    {
        [Required]
        [Display(Name = "VacationOfferId")]
        public int VacationOfferId { get; set; }
        [Required]
        [Display(Name = "VacationRequestId")]
        public int VacationRequestId { get; set; }
        [Required]
        [Display(Name = "EmployeeEmail")]
        public string EmployeeEmail { get; set; } = string.Empty;
        [Required]
        [Display(Name = "TotalPrice")]
        public decimal TotalPrice { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; } = string.Empty;
        [Required]
        [Display(Name = "StartDate")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Enddate")]
        public DateTime EndDate { get; set; }
        
        [Display(Name = "Misc")]
        public string? Misc { get; set; }
        [Required]
        [Display(Name = "ExpirationDate")]
        public DateTime ExpirationDate { get; set; }
    }
}
