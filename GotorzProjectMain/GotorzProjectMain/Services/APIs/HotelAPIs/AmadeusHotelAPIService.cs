using AutoMapper;
using System.Text.Json;
using System.Web;
using GotorzProjectMain.Models;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Net;
using Azure.Core;
using Azure;
using GotorzProjectMain.InputModels.HotelAPIInputModels;
using GotorzProjectMain.InputModels.HotelAPIInputModels;
using GotorzProjectMain.Services.APIs.HotelAPIs.DTO;

namespace GotorzProjectMain.Services.APIs.HotelAPIs;

public class AmadeusHotelAPIService : IAmadeusHotelAPIService
{
    public string? ApiResponseInfoMessage { get; set; }
    public string? ErrorMessage { get; set; }
    public IEnumerable<Hotel> Hotels { get; set; } = Enumerable.Empty<Hotel>();
    public List<string> HotelIds { get; set; } = [];

    // Singleton som indeholder hemmelig API-info
    private readonly AmadeusSettings _settings;
    private readonly HttpClient _client;
    private readonly IMapper _mapper;
    private readonly ILogger<AmadeusHotelAPIService> _logger;
    private readonly IRateLimiter _rateLimiter;
    private int _batch;
    private string? _accessToken;
    private DateTime? _tokenExpiresAt;

    // Nested record hvor navnene passer med incoming json, så det kan deserialiseres. 
    private sealed record AmadeusTokenResponse(
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("expires_in")] int ExpiresIn
        );

    public AmadeusHotelAPIService(HttpClient client, IMapper mapper, AmadeusSettings settings, ILogger<AmadeusHotelAPIService> logger, IRateLimiter rateLimiter)
    {
        _client = client;
        _mapper = mapper;
        _settings = settings;
        _logger = logger;
        _rateLimiter = rateLimiter;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        // Hvis tidligere token stadig er gyldig, så brug den.
        if (_accessToken != null && DateTime.UtcNow < _tokenExpiresAt)
        {
            return _accessToken;
        }

        // Ellers:
        // Byg HTTP request
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.amadeus.com/v1/security/oauth2/token");
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "grant_type", "client_credentials" },
        { "client_id", _settings.ApiKey! },
        { "client_secret", _settings.ApiSecret! }
    });

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        // Deserialisering
        var json = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<AmadeusTokenResponse>(json)!;

        // Sæt værdier til feltvariabler
        _accessToken = token.AccessToken;
        _tokenExpiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn - 60); // 60s buffer

        return _accessToken;
    }

    public async Task SearchHotelsAsync(AmadeusHotelListInputModel listParameters, AmadeusHotelOfferInputModel offerParameters)
    {
        try
        {
            ErrorMessage = null;
            ApiResponseInfoMessage = null;

            if (!_rateLimiter.TryRequest())
            {
                throw new InvalidOperationException("Rate limit exceeded");
            }

            var token = await GetAccessTokenAsync();

            // Tjekker om det er et nyt kald eller kald efter flere offers på sidste kald
            if (!HotelIds.Any())
            {
                _batch = 0;
                var listQuery = BuildListQueryAsync(listParameters);

                // Byg HTTP request
                var listRequest = new HttpRequestMessage(HttpMethod.Get, $"v1/reference-data/locations/hotels/by-city?{listQuery}");
                listRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                // Fortæller hvilken api version vi bruger. Hvis den ændrer sig, tror jeg det her smider en eller anden form for fejl - noget i den stil.
                listRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.amadeus+json"));

                var listResponse = await _client.SendAsync(listRequest);
                var listJson = await listResponse.Content.ReadAsStringAsync();

                // Intern fejlkodehåndtering
                if (listResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    _logger.LogWarning($"Bad Request: {listJson}");

                    var doc = JsonDocument.Parse(listJson);
                    var errorCode = doc.RootElement
                        .GetProperty("errors")[0]
                        .GetProperty("code")
                        .GetInt32();

                    if (errorCode == 1157)
                    {
                        ApiResponseInfoMessage = "Invalid city code - Amadeus backend occasionally fakes this when it chokes on valid city codes. Known issue. Not your fault. Retry manually in a minute or light a candle.";
                    }
                    else if (errorCode == 895)
                    {
                        ApiResponseInfoMessage = "Nothing found for the requested criteria";
                    }
                    else
                    {
                        ApiResponseInfoMessage = "Bad request response on list-api";
                    }

                    return;
                }

                listResponse.EnsureSuccessStatusCode();

                // Deserialisering
                var listDto = JsonSerializer.Deserialize<HotelSearchResultDto>(listJson, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                });

                // Mapping
                Hotels = listDto?.Data != null ? _mapper.Map<IEnumerable<Hotel>>(listDto.Data) : Enumerable.Empty<Hotel>();

                if (!Hotels.Any())
                {
                    ApiResponseInfoMessage = "No hotels matched the search. Try a broader search.";

                    return;
                }

                // Sæt hotelId'er
                HotelIds = Hotels.Select(h => h.HotelId!).ToList();
            }

            // Batcher hotelOffer søgningen hvis der er flere end 50 hoteller
            List<string> hotelIdsBatch;
            if (HotelIds.Count() > 50)
            {
                hotelIdsBatch = HotelIds.Take(50).ToList();
                HotelIds.RemoveRange(0, hotelIdsBatch.Count);
            }
            else
            {
                hotelIdsBatch = HotelIds.ToList();
                HotelIds.Clear();
            }

            var offerQuery = BuildOffersQueryAsync(offerParameters, hotelIdsBatch);

            // Byg HTTP request
            var offerRequest = new HttpRequestMessage(HttpMethod.Get, $"v3/shopping/hotel-offers?{offerQuery}");
            offerRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // Fortæller hvilken api version vi bruger. Hvis den ændrer sig, tror jeg det her smider en eller anden form for fejl - noget i den stil.
            offerRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.amadeus+json"));

            var offerResponse = await _client.SendAsync(offerRequest);
            var offerJson = await offerResponse.Content.ReadAsStringAsync();

            // Hvis ingen offers findes (eller noget andet halløj), sættes information herom
            if (offerResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                _logger.LogWarning($"Bad Request: {offerJson}");

                var doc = JsonDocument.Parse(offerJson);
                var errorCode = doc.RootElement
                    .GetProperty("errors")[0]
                    .GetProperty("code")
                    .GetInt32();

                if (errorCode == 3664)
                {
                    ApiResponseInfoMessage = "No offers available for the selected hotels";
                }
                else if (errorCode == 1257)
                {
                    ApiResponseInfoMessage = "Missing hotel IDs";
                }
                else if (errorCode == 477)
                {
                    ApiResponseInfoMessage = "Invalid format";
                }
                else
                {
                    ApiResponseInfoMessage = "Bad request response on offers-api";
                }

                return;
            }

            offerResponse.EnsureSuccessStatusCode();

            // Deserialisering
            var searchDto = JsonSerializer.Deserialize<HotelOffersResponseDto>(offerJson, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });

            // Næste batch (.Take(50) smider ikke error selvom der er færre end 50)
            foreach (Hotel hotel in Hotels.Skip(_batch * 50).Take(50))
            {
                // Match parent-offerDto til hotelobjekt ud fra hotelId
                HotelOfferDataDto? hotelOffers = searchDto?.Data?.FirstOrDefault(h => h?.Hotel?.HotelId == hotel.HotelId);
                // Map
                hotel.Offers = hotelOffers?.Offers != null ? _mapper.Map<IReadOnlyList<HotelOffer>>(hotelOffers.Offers) : Array.Empty<HotelOffer>();
            }

            // Hvis der stadig er flere hoteller at søge tilbud i lægges 1 til batch for at alt stemmer ved kald efter flere tilbud.
            if (HotelIds.Any())
            {
                _batch++;
            }
        }
        catch (InvalidOperationException)
        {
            ErrorMessage = $"Rate limit per minute exceeded, try again in 1 minute.";
        }
        catch (Exception ex)
        {
            if (string.IsNullOrWhiteSpace(ErrorMessage))
            {
                ErrorMessage = $"An unexpected error occurred: {ex}";
            }

            _logger.LogError(ex, ErrorMessage);
        }
    }

    public string BuildListQueryAsync(AmadeusHotelListInputModel p)
    {
        try
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            foreach (var prop in typeof(AmadeusHotelListInputModel).GetProperties())
            {
                // hvis propertyen ikke har en JsonPropertyName, skal det ikke med i queryen
                var attribute = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                if (attribute == null) continue;

                // Hent værdi
                var value = prop.GetValue(p);

                // Hvis ingen værdi, skal de ikke sættes. Ellers sæt værdierne i query-byggeren.
                if (value is null) continue;

                switch (value)
                {
                    case string s when !string.IsNullOrWhiteSpace(s):
                        query[attribute.Name] = s;
                        break;

                    case int i:
                        query[attribute.Name] = i.ToString();
                        break;

                    case List<string> list when list.Any():
                        query[attribute.Name] = string.Join(",", list);
                        break;
                }
            }

            return query.ToString()!;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred building offer query: {ex}";
            throw;
        }
    }

    public string BuildOffersQueryAsync(AmadeusHotelOfferInputModel p, List<string> hotelIds)
    {
        try
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            // Sæt hotelId'er
            query["hotelIds"] = string.Join(",", hotelIds);

            foreach (var prop in typeof(AmadeusHotelOfferInputModel).GetProperties())
            {
                var attribute = prop.GetCustomAttribute<JsonPropertyNameAttribute>();

                // hotelId allerede håndteret
                if (attribute == null || attribute.Name == "hotelIds") continue;

                // Hent værdi
                var value = prop.GetValue(p);

                // Hvis ingen værdi, skal de ikke sættes. Ellers sæt værdierne i query-byggeren.
                if (value == null) continue;

                switch (value)
                {
                    case string s when !string.IsNullOrWhiteSpace(s):
                        query[attribute.Name] = s;
                        break;

                    case int i:
                        query[attribute.Name] = i.ToString();
                        break;

                    case bool b:
                        query[attribute.Name] = b.ToString().ToLower();
                        break;

                    case DateOnly d:
                        query[attribute.Name] = d.ToString("yyyy-MM-dd");
                        break;

                    case List<string> list when list.Any():
                        query[attribute.Name] = string.Join(",", list);
                        break;
                }
            }

            return query.ToString()!;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred building offer query: {ex}";
            throw;
        }
    }
}
