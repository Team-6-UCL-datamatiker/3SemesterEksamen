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
			int children = 0,
			bool deepSearch = false);

		Task<string> GetRawJsonAsync(
			string departureIata,
			string arrivalIata,
			DateTime outboundDate,
			bool deepSearch = false);
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

		public async Task<List<Flight>> SearchAsync(
			string departureIata,
			string arrivalIata,
			DateTime outboundDate,
			int adults = 1,
			int children = 0,
			bool deepSearch = false)
		{
			// Prepare the query parameters
			var query = new Dictionary<string, string>
			{
				["engine"] = "google_flights",
				["api_key"] = _apiKey,
				["departure_id"] = departureIata,
				["arrival_id"] = arrivalIata,
				["outbound_date"] = outboundDate.ToString("yyyy-MM-dd"),
				["deep_search"] = deepSearch.ToString().ToLowerInvariant(),
				["adults"] = adults.ToString(), 
				["children"] = children.ToString(),
				["currency"] = "DKK",
				["type"] = "2" // For the api to know it's a oneway (needed)
			};

			// Add the query string to the URL
			var url = QueryHelpers.AddQueryString("search", query);
			Console.WriteLine($"Generated URL: {url}"); // Log the URL for debugging

			// Make the GET request to the API
			var resp = await _http.GetFromJsonAsync<SerpApiFlightsResponse>(url);

			if (resp == null) return new();

			// The API returns a list of flight groups, each containing a list of flights
			var groups = (resp.BestFlights ?? new())
					   .Concat(resp.OtherFlights ?? new());

			// Flatten the list of flight groups into a single list of flights
			var offers = new List<Flight>();

			// Each group contains a list of flights, we need to extract them
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
						Airline = s.Airline,
						BookingLink = s.BookingLink
					});
			return offers;
		}

		// Debug helper: returns the raw JSON
		public async Task<string> GetRawJsonAsync(
			string departureIata,
			string arrivalIata,
			DateTime outboundDate,
			bool deepSearch = false)
		{
			var query = new Dictionary<string, string>
			{
				["engine"] = "google_flights",
				["api_key"] = _apiKey,
				["departure_id"] = departureIata,
				["arrival_id"] = arrivalIata,
				["outbound_date"] = outboundDate.ToString("yyyy-MM-dd"),
				["deep_search"] = deepSearch.ToString().ToLowerInvariant()
			};

			var url = QueryHelpers.AddQueryString("search", query);
			return await _http.GetStringAsync(url);
		}
	}
}
