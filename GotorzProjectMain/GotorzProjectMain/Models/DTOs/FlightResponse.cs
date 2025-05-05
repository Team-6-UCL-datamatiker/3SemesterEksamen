using System.Text.Json.Serialization;

namespace GotorzProjectMain.Models.DTOs
{
	public class FlightResponseDTO
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
