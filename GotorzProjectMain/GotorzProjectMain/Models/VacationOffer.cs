using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GotorzProjectMain.Models
{
    public class VacationOffer
    {
        [Key]
        public int VacationOfferId { get; set; }

        [ForeignKey("VacationRequest")]
        public int VacationRequestId { get; set; }

        public VacationRequest VacationRequest { get; set; }

        public string EmployeeEmail { get; set; }

        public float TotalPrice { get; set; }

        public string DepartureCountry { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCountry { get; set; }
        public string ArrivalCity { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Misc { get; set; }
        
        public OfferStatus OfferStatus { get; set; } = OfferStatus.Waiting;
        
        public DateTime ExpirationDate { get; set; }
    }

    public enum OfferStatus
    {
        Waiting,
        Approved,
        Denied
    }
}
