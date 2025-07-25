﻿using AutoMapper;
using GotorzProjectMain.Models;
using GotorzProjectMain.Services.APIs.FlightAPI.DTO;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;

namespace GotorzProjectMain.Services.APIs.FlightAPI
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
        private readonly IMapper _mapper;

        public FlightService(HttpClient http, IConfiguration cfg, IMapper mapper)
        {
            _http = http;
            _apiKey = cfg["SerpApi:ApiKey"]!;
            _mapper = mapper;
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
            FlightResponseDTO? response = await _http.GetFromJsonAsync<FlightResponseDTO>(url); // equivalent to a GET + Deserialize
            if (response == null) return new();

            // The API returns a list of flight routesDTO, each containing a list of flights - combine them
            IEnumerable<FlightRouteDTO> routesDTO = (response.BestFlights ?? new())
                       .Concat(response.OtherFlights ?? new());

            List<FlightRoute> routes = routesDTO
            .Select(routeDTO =>
            {
                var route = _mapper.Map<FlightRoute>(routeDTO);
                return route;
            })
            .ToList();

            return routes;
        }
    }
}
