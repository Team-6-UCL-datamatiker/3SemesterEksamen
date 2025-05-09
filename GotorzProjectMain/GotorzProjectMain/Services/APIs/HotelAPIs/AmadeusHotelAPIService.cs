using System.Globalization;
using System.Net.Http;
using AutoMapper;
using System.Text.Json;
using System.Web;
using GotorzProjectMain.Models;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Net;

namespace GotorzProjectMain.Services.APIs.HotelAPIs;

public class AmadeusHotelAPIService : IAmadeusHotelAPIService
{
    private readonly HttpClient _client;
    private readonly IMapper _mapper;
    private readonly AmadeusSettings _settings;

    private string? _accessToken;
    private DateTime _expiresAt;
    private string? _message = "";

    public AmadeusHotelAPIService(HttpClient client, IMapper mapper, AmadeusSettings settings)
    {
        _client = client;
        _mapper = mapper;
        _settings = settings;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        if (_accessToken != null && DateTime.UtcNow < _expiresAt)
        {
            return _accessToken;
        }

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.amadeus.com/v1/security/oauth2/token");
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "grant_type", "client_credentials" },
        { "client_id", _settings.ApiKey! },
        { "client_secret", _settings.ApiSecret! }
    });

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<AmadeusTokenResponse>(json)!;

        _accessToken = token.AccessToken;
        _expiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn - 60); // subtract 60s buffer

        return _accessToken;
    }

    public async Task<(IEnumerable<Hotel>, string)> SearchHotelsAsync(AmadeusHotelListParameters listParameters, AmadeusHotelSearchParameters searchParameters)
    {
        var query = BuildListQuery(listParameters);

        var token = await GetAccessTokenAsync();

        var listRequest = new HttpRequestMessage(HttpMethod.Get, $"v1/reference-data/locations/hotels/by-city?{query}");
        listRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        listRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.amadeus+json"));

        var response = await _client.SendAsync(listRequest);

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        var listDto = JsonSerializer.Deserialize<HotelSearchResultDto>(json, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        });

        var hotels = listDto?.Data != null ? _mapper.Map<IEnumerable<Hotel>>(listDto.Data) : Enumerable.Empty<Hotel>();

        if (!hotels.Any())
        {
            return (hotels, _message!);
        }

        List<string> hotelIds = [];
        foreach (Hotel hotel in hotels)
        {
            if (hotel.HotelId != null)
            {
                hotelIds.Add(hotel.HotelId);
            }
        }

        // MIDLERTIDIGT FIX:
        if (hotels.Count() > 10)
        {
            hotelIds = hotelIds.Take(10).ToList();
        }
        // --------------------------------------------------------------------------------------------------------

        query = BuildSearchQuery(searchParameters, hotelIds);

        var searchRequest = new HttpRequestMessage(HttpMethod.Get, $"v3/shopping/hotel-offers?{query}");
        searchRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.amadeus+json"));

        response = await _client.SendAsync(searchRequest);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var badJson = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(badJson);
            var errorCode = doc.RootElement
                .GetProperty("errors")[0]
                .GetProperty("code")
                .GetInt32();

            if (errorCode == 3664)
            {
                _message = "No offers available for the selected hotels";
            }
        }
        else
        {
            response.EnsureSuccessStatusCode();
            json = await response.Content.ReadAsStringAsync();

            var searchDto = JsonSerializer.Deserialize<HotelOffersResponseDto>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });

            HotelOfferDataDto? hotelOffers;

            foreach (Hotel hotel in hotels)
            {
                hotelOffers = searchDto?.Data?.FirstOrDefault(h => h?.Hotel?.HotelId == hotel.HotelId);
                hotel.Offers = hotelOffers?.Offers != null ? _mapper.Map<IReadOnlyList<HotelOffer>>(hotelOffers.Offers) : Array.Empty<HotelOffer>();
            }
        }

        return (hotels, _message!);
    }

    public string BuildListQuery(AmadeusHotelListParameters p)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);

        foreach (var prop in typeof(AmadeusHotelListParameters).GetProperties())
        {
            var attr = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
            if (attr == null) continue;

            var name = attr.Name;
            var value = prop.GetValue(p);

            if (value is null) continue;

            switch (value)
            {
                case string s when !string.IsNullOrWhiteSpace(s):
                    query[name] = s;
                    break;
                case int i when !(prop.Name == nameof(p.Radius) && i == 5):
                    query[name] = i.ToString();
                    break;
                case List<string> list when list.Any():
                    query[name] = string.Join(",", list);
                    break;
            }
        }

        return query.ToString();
    }

    public string BuildSearchQuery(AmadeusHotelSearchParameters p, List<string> hotelIds)
    {
        try
        {
            var q = HttpUtility.ParseQueryString(string.Empty);
            q["hotelIds"] = string.Join(",", hotelIds);

            foreach (var prop in typeof(AmadeusHotelSearchParameters).GetProperties())
            {
                var attr = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                if (attr == null || attr.Name == "hotelIds") continue; // already handled

                var v = prop.GetValue(p);
                if (v == null) continue;

                switch (v)
                {
                    case string s when !string.IsNullOrWhiteSpace(s):
                        q[attr.Name] = s;
                        break;

                    case int i:
                        q[attr.Name] = i.ToString();
                        break;

                    //case bool b when b:
                    //    q[attr.Name] = "true";
                    //    break;

                    case DateOnly d:
                        q[attr.Name] = d.ToString("yyyy-MM-dd");
                        break;

                    case List<string> list when list.Any():
                        q[attr.Name] = string.Join(",", list);
                        break;
                }
            }

            return q.ToString()!;
        }
        catch
        {
            // Do something here.
            throw;
        }
    }

    public sealed record AmadeusTokenResponse(
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("expires_in")] int ExpiresIn);
}
