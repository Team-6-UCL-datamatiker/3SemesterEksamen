using GotorzProjectMain.Models.DTOs;

namespace GotorzProjectMain.Models
{
	public class FlightOffer
	{
		// All legs in this itinerary, in order
		public List<Flight> Segments { get; set; }

		// Layover info between legs
		public List<Layover> Layovers { get; set; }

		// Total price across all legs
		public decimal TotalPrice { get; set; }

		// Total traveltime (across all legs and layovers)
		public TimeSpan TotalTravelTime
		{
			get
			{
				// Sum each flight’s duration from departure→arrival
				var flightMinutes = Segments
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
