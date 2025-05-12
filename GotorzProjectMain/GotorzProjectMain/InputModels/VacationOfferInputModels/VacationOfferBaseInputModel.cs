using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.InputModels.VacationOfferInputModels
{
    public class VacationOfferBaseInputModel
    {
        [Required]
        [Display(Name = "EmployeeEmail")]
        public string EmployeeEmail { get; set; } = string.Empty;
        [Required]
        [Display(Name = "TotalPrice")]
        public decimal TotalPrice { get; set; }
        [Required]
        [Display(Name = "DepartureCountry")]
        public string DepartureCountry { get; set; } = string.Empty;
        [Required]
        [Display(Name = "DepartureCity")]
        public string DepartureCity { get; set; } = string.Empty;
        [Required]
        [Display(Name = "ArrivalCountry")]
        public string ArrivalCountry { get; set; } = string.Empty;
        [Required]
        [Display(Name = "ArrivalCity")]
        public string ArrivalCity { get; set; } = string.Empty;
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
