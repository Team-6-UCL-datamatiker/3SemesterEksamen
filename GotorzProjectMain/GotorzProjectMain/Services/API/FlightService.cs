using GotorzProjectMain.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;

namespace GotorzProjectMain.Services.API
{
	public interface IFlightService
	{
		Task<List<Flight>> SearchAsync(
			string departureIata,
			string arrivalIata,
			DateTime outboundDate,
			int adults = 1,
			int children = 0);
	}


	public class FlightService : IFlightService
	{
		private readonly HttpClient _http;
		private readonly string _apiKey;
		public FlightService(HttpClient http, IConfiguration cfg)
		{
			_http = http;
			_apiKey = cfg["SerpApi:ApiKey"]!;
		}

		// Fetches available flights from API using Airport codes
		public async Task<List<Flight>> SearchAsync(
			string departureIata,
			string arrivalIata,
			DateTime outboundDate,
			int adults = 1,
			int children = 0)
		{
			// Build request parameters for API
			var query = new Dictionary<string, string>
			{
				["engine"] = "google_flights",
				["api_key"] = _apiKey,
				["departure_id"] = departureIata,
				["arrival_id"] = arrivalIata,
				["outbound_date"] = outboundDate.ToString("yyyy-MM-dd"),
				["adults"] = adults.ToString(), 
				["children"] = children.ToString(),
				["currency"] = "DKK",
				["type"] = "2" // Oneway flight (needed to not give error when not giving a return date)
			};

			// Construct request URL with parameters (search?engine=google_flight...)
			var url = QueryHelpers.AddQueryString("search", query);

			// Fetch the JSON from API and deserialize it into DTO 
			var response = await _http.GetFromJsonAsync<SerpApiFlightsResponse>(url); // equivalent to a GET + Deserialize
			if (response == null) return new();

			// The API returns a list of flight groups, each containing a list of flights - combine them
			var groups = (response.BestFlights ?? new())
					   .Concat(response.OtherFlights ?? new());

			// Convert DTO into to Flight Model
			List<Flight> offers = new List<Flight>();
			foreach (var g in groups)
				foreach (var s in g.Flights)
					offers.Add(new Flight
					{
						DepartureAirportCode = s.Departure.Id,
						DepartureAirportName = s.Departure.Name,
						DepartureTime = DateTime.Parse(s.Departure.Time, null,
												  DateTimeStyles.RoundtripKind),
						ArrivalAirportCode = s.Arrival.Id,
						ArrivalAirportName = s.Arrival.Name,
						ArrivalTime = DateTime.Parse(s.Arrival.Time, null,
												  DateTimeStyles.RoundtripKind),
						Price = g.Price,
						Airline = s.Airline
					});
			return offers;
		}		
	}
}
