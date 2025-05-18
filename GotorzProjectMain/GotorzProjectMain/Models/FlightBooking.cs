using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GotorzProjectMain.Models
{
    public class FlightBooking
    {
        public int FlightBookingId { get; set; }
        public ICollection<FlightRoute> FlightRoutes { get; set; } = [];
        //public string? Airline { get; set; }
        //public int? SeatNumber { get; set; }
        public float? TotalPrice { get; set; }
        public string? BookingInformationLink { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public string? Misc { get; set; }
        public string? DepartureAirport { get; set; }
        public string? ArrivalAirport { get; set; }
        public int VacationOfferId { get; set; }
    }
}
