using GotorzProjectMain.Services.APIs.FlightAPI.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GotorzProjectMain.Models
{
	public class FlightRoute
	{
		// Unique identifier for this route
        public int RouteId { get; set; }
		// The flight booking associated with this route
        public int FlightBookingId { get; set; }
        // All legs in this route, in order
        public List<Flight> Legs { get; set; }
		// Layover info between legs
		public List<Layover> Layovers { get; set; }
		// Total price across all legs
		public float TotalPrice { get; set; }
		// Total traveltime (across all legs and layovers)
		public TimeSpan TotalTravelTime
		{
			get
			{
				// Sum each flight’s duration from departure→arrival
				var flightMinutes = Legs
					.Sum(s => (s.ArrivalTime - s.DepartureTime).TotalMinutes);

				// Sum each layover’s duration in minutes
				var layoverMinutes = Layovers?.Sum(l => l.Duration) ?? 0;

				return TimeSpan.FromMinutes(flightMinutes + layoverMinutes);
			}
		}
		// for display purposes
		public string TotalTravelTimeDisplay
		=> $"{(int)TotalTravelTime.TotalHours}h {TotalTravelTime.Minutes}m";
	}
}
