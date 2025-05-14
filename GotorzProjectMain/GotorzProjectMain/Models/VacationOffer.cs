using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GotorzProjectMain.Models
{
    public class VacationOffer
    {
        public int VacationOfferId { get; set; }
        public int VacationRequestId { get; set; }
        public string EmployeeEmail { get; set; }
        public float TotalPrice { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public string Misc { get; set; }        
        public OfferStatus OfferStatus { get; set; } = OfferStatus.Waiting;
        public DateTime ExpirationDate { get; set; } = DateTime.Now;
        public HotelBooking? HotelBooking { get; set; }
        public FlightBooking? FlightBooking { get; set; }
    }

    public enum OfferStatus
    {
        Waiting,
        Approved,
        Denied
    }
}
