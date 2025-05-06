using System.Globalization;
using System.Net.Http;
using AutoMapper;
using System.Text.Json;
using System.Web;
using GotorzProjectMain.Models;
using System.Net.Http.Headers;

namespace GotorzProjectMain.Services.APIs.HotelAPIs;

public class AmadeusHotelAPIService : IAmadeusHotelAPIService
{
    private readonly HttpClient _client;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private string _key;
    private string _secret;

    public AmadeusHotelAPIService(HttpClient client, IConfiguration config ,IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
        _config = config;
        _key = _config["AmadeusApi:ApiKey"];
        _secret = _config["AmadeusApi:ClientSecret"];
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://test.api.amadeus.com/v1/security/oauth2/token");
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "grant_type", "client_credentials" },
        { "client_id", _key },
        { "client_secret", _secret }
    });

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("access_token").GetString();
    }

    public async Task<IEnumerable<Hotel>> SearchHotelsAsync(AmadeusHotelListParameters parameters)
    {
        var query = BuildQuery(parameters);

        var token = await GetAccessTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Get, $"reference-data/locations/hotels/by-city?{query}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.amadeus+json"));

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        var dto = JsonSerializer.Deserialize<HotelSearchResultDto>(json, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        });

        return dto?.Data != null ? _mapper.Map<IEnumerable<Hotel>>(dto.Data) : Enumerable.Empty<Hotel>();
    }

    public string BuildQuery(AmadeusHotelListParameters p)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["cityCode"] = p.CityOrAirportCode;

        if (p.Radius != 5)
        {
            query["radius"] = p.Radius.ToString();
        }
        if (p.RadiusUnit != "KM")
        {
            query["radiusUnit"] = p.RadiusUnit;
        }
        if (p.ChainCodes?.Any() == true)
        {
            query["chainCodes"] = string.Join(",", p.ChainCodes);
        }
        if (p.Amenities?.Any() == true)
        {
            query["amenities"] = string.Join(",", p.Amenities);
        }
        if (p.Ratings?.Any() == true)
        {
            query["ratings"] = string.Join(",", p.Ratings);
        }
        if (!string.IsNullOrWhiteSpace(p.HotelSource))
        {
            query["hotelSource"] = p.HotelSource;
        }
        return query.ToString();
    }
}
