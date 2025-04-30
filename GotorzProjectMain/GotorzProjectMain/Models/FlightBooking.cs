using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GotorzProjectMain.Models
{
    public class FlightBooking
    {
        [Key]
        public int FlightBookingId { get; set; }

        public string Airline { get; set; }

        public int SeatNumber { get; set; }

        public float Price { get; set; }

        public string BookingInformationLink { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public string Misc { get; set; }

        public string DepartureAirport { get; set; }

        public string ArrivalAirport { get; set; }

        [ForeignKey("VacationOffer")]
        public int VacationOfferId { get; set; }

        public VacationOffer VacationOffer { get; set; }
    }
}
