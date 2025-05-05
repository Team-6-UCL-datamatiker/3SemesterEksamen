using System.Text.Json.Serialization;

namespace GotorzProjectMain.Models
{
	public class SerpApiFlightsResponse
	{
		[JsonPropertyName("best_flights")]
		public List<FlightGroup> BestFlights { get; set; }
		[JsonPropertyName("other_flights")]
		public List<FlightGroup> OtherFlights { get; set; }
	}
	public class FlightGroup
	{
		public List<FlightSegment> Flights { get; set; }

		[JsonPropertyName("layovers")]
		public List<Layover> Layovers { get; set; }

		[JsonPropertyName("price")]
		public decimal Price { get; set; }
	}
	public class FlightSegment
	{
		[JsonPropertyName("departure_airport")]
		public AirportInfo Departure { get; set; }
		[JsonPropertyName("arrival_airport")]
		public AirportInfo Arrival { get; set; }
		public string Airline { get; set; }
		[JsonPropertyName("booking_link")]
		public string BookingLink { get; set; }
	}
	public class AirportInfo
	{
		public string Name { get; set; }
		public string Id { get; set; }   // IATA code
		public string Time { get; set; }   // ISO-8601 string
	}

	// For Grouping flights legs into a single offer
	public class FlightOffer
	{
		// All legs in this itinerary, in order
		public List<Flight> Segments { get; set; }

		// Layover info between legs
		public List<Layover> Layovers { get; set; }

		// Total price across all legs
		public decimal TotalPrice { get; set; }

		// Total traveltime (across all legs and layovers)
		[JsonIgnore]
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

		[JsonIgnore]
		public string TotalTravelTimeDisplay
		=> $"{(int)TotalTravelTime.TotalHours}h {TotalTravelTime.Minutes}m";
	}
		
	public class Layover
	{
		public string Id { get; set; }  // airport code
		public string Name { get; set; }
		public int Duration { get; set; }  // minutes

		[JsonIgnore]
		public string DurationDisplay
		=> $"{Duration / 60}h {Duration % 60}m"; // hours and minutes
	}

}
