using GotorzProjectMain.Models;
using GotorzProjectMain.Models.DTOs;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;

namespace GotorzProjectMain.Services.API
{
	public interface IFlightService
	{
		Task<List<FlightRoute>> SearchAsync(
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
		public async Task<List<FlightRoute>> SearchAsync(
			string departureIata,
			string arrivalIata,
			DateTime outboundDate,
			int adults = 1,
			int children = 0)
		{
			// Build request parameters for API
			Dictionary<string, string> query = new()
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
			string url = QueryHelpers.AddQueryString("search", query);

			// Fetch the JSON from API and deserialize it into DTO 
			FlightResponseDTO response = await _http.GetFromJsonAsync<FlightResponseDTO>(url); // equivalent to a GET + Deserialize
			if (response == null) return new();

			// The API returns a list of flight groups, each containing a list of flights - combine them
			IEnumerable<FlightGroup> groups = (response.BestFlights ?? new())
					   .Concat(response.OtherFlights ?? new());

			List<FlightRoute> routes = new();

			// Convert DTO into to Flight Model
			foreach (FlightGroup group in groups)
			{
				// Map each FlightSegment → Flight
				List<Flight> legs = group.Flights
							.Select(leg => new Flight
							{
								DepartureAirportCode = leg.Departure.Id,
								DepartureAirportName = leg.Departure.Name,
								DepartureTime = DateTime.Parse(leg.Departure.Time, null, DateTimeStyles.RoundtripKind),
								ArrivalAirportCode = leg.Arrival.Id,
								ArrivalAirportName = leg.Arrival.Name,
								ArrivalTime = DateTime.Parse(leg.Arrival.Time, null, DateTimeStyles.RoundtripKind),
								Price = group.Price,
								Airline = leg.Airline,
								BookingLink = leg.BookingLink
							})
							.ToList();

				// Map each layover DTO → Layover
			
				List<Layover> layovers = group.Layovers?.ToList()
							 ?? new();

				// Wrap into FlightRoute
				routes.Add(new FlightRoute
				{
					Segments = legs,
					Layovers = layovers,
					TotalPrice = group.Price
				}); 
			}
			return routes;
		}
	}
}
