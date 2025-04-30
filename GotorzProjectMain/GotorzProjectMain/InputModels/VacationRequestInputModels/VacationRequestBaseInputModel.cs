using GotorzProjectMain.Models;
using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.InputModels.VacationRequestInputModels
{
    public class VacationRequestBaseInputModel
    {
        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.Now;
		[Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; } = DateTime.Now;

		[Display(Name = "Children")]
        [Range(0, int.MaxValue, ErrorMessage = "Children cannot be below 0")]
        public int ChildrenAmount { get; set; }

        [Display(Name = "Adults")]
        [Range(1, int.MaxValue, ErrorMessage = "Adults cannot be below 1")]
        public int AdultsAmount { get; set; }

        [Display(Name = "Rooms")]
        [Range(1, int.MaxValue, ErrorMessage = "Rooms cannot be below 1")]
        public int RoomsAmount { get; set; }

        [Display(Name = "Hotel Request")]
        public string HotelRequest { get; set; }

        [Display(Name = "Flight Request")]
        public string FlightRequest { get; set; }

        [Display(Name = "Other")]
        public string Misc { get; set; }

        
    }
}
