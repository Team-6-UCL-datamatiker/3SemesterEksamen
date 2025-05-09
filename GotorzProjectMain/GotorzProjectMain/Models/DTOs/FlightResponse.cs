using System.Text.Json.Serialization;

namespace GotorzProjectMain.Models.DTOs
{
	public class FlightResponseDTO
	{
		[JsonPropertyName("best_flights")]
		public List<FlightRouteDTO> BestFlights { get; set; }
		[JsonPropertyName("other_flights")]
		public List<FlightRouteDTO> OtherFlights { get; set; }
	}
	public class FlightRouteDTO
	{
		public List<FlightLegDTO> Flights { get; set; }

		[JsonPropertyName("layovers")]
		public List<LayoverDTO> Layovers { get; set; }

		[JsonPropertyName("price")]
		public decimal Price { get; set; }
	}
	public class FlightLegDTO
	{
		[JsonPropertyName("departure_airport")]
		public AirportInfoDTO Departure { get; set; }
		[JsonPropertyName("arrival_airport")]
		public AirportInfoDTO Arrival { get; set; }
		public string Airline { get; set; }
	}
	public class AirportInfoDTO
	{
		public string Name { get; set; }
		public string Id { get; set; }   // IATA code
		public string Time { get; set; }   // ISO-8601 string
	}

		
	public class LayoverDTO
	{
        public int LayoverId { get; set; }
        //public string Id { get; set; }  // airport code
		public string Name { get; set; }
		public int Duration { get; set; }  // minutes
	}

}
