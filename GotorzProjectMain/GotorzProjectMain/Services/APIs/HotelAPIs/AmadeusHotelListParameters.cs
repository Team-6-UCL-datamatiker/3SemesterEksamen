using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GotorzProjectMain.Models;
using Humanizer;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;

namespace GotorzProjectMain.Services.APIs.HotelAPIs;

public class AmadeusHotelListParameters
{
    // DESTINATION CITY CODE OR AIRPORT CODE
    // In case of city code, the search will be done around the city center.
    // Available codes can be found in IATA table codes (3 chars IATA Code).
    [JsonPropertyName("cityCode")]
    [Required]
    public string CityOrAirportCode { get; set; } = string.Empty;

    // MAXIMUM DISTANCE FROM THE GEOGRAPHICAL COORDINATES EXPRESS IN DEFINED UNITS
    // Default: 5
    [JsonPropertyName("radius")]
    [Range(1, 50, ErrorMessage = "Must be between 1 and 50")]
    public int Radius { get; set; } = 3;

    // UNIT OF MEASUREMENT
    // Default: KM (Available values : KM, MILE)
    [JsonPropertyName("radiusUnit")]
    public string RadiusUnit { get; set; } = "KM";

    // ARRAY OF HOTEL CHAIN CODES
    // Each code is a string consisted of 2 capital alphabetic characters.
    [JsonPropertyName("chainCodes")]
    public List<string>? ChainCodes { get; set; }

    // LIST OF AMENITIES
    // Available values:
    // SWIMMING_POOL, SPA, FITNESS_CENTER, AIR_CONDITIONING, RESTAURANT, PARKING, PETS_ALLOWED,
    // AIRPORT_SHUTTLE, BUSINESS_CENTER, DISABLED_FACILITIES, WIFI, MEETING_ROOMS, NO_KID_ALLOWED,
    // TENNIS, GOLF, KITCHEN, ANIMAL_WATCHING, BABY-SITTING, BEACH, CASINO, JACUZZI, SAUNA, SOLARIUM,
    // MASSAGE, VALET_PARKING, BAR or LOUNGE, KIDS_WELCOME, NO_PORN_FILMS, MINIBAR, TELEVISION,
    // WI-FI_IN_ROOM, ROOM_SERVICE, GUARDED_PARKG, SERV_SPEC_MENU
    [JsonPropertyName("amenities")]
    public List<string> Amenities { get; set; } = new();

    // HOTEL STARS
    // Up to four values can be requested at the same time in a comma separated list.
    // Available values : 1, 2, 3, 4, 5
    [JsonPropertyName("ratings")]
    public List<string> Ratings { get; set; } = new();

    // Hotel source with values BEDBANK for aggregators, DIRECTCHAIN for GDS/Distribution and ALL for both.
    // Available values : BEDBANK, DIRECTCHAIN, ALL
    // Default: ALL
    [JsonPropertyName("hotelSource")]
    public string? HotelSource { get; set; }
}