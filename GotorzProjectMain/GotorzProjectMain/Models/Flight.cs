using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GotorzProjectMain.Models
{
	public class Flight
	{
		[Key]
		public int FlightId { get; set; }

        [ForeignKey("FlightRoute")]
        public int FlightRouteId { get; set; }

        public string DepartureAirportCode { get; set; }
		public string DepartureAirportName { get; set; }
		public string ArrivalAirportCode { get; set; }
		public string ArrivalAirportName { get; set; }
		public DateTime DepartureTime { get; set; }
		public DateTime ArrivalTime { get; set; }
		public string Airline { get; set; }
	}
}
