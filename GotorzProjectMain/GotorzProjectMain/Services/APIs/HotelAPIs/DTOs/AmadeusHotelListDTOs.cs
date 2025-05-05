using System.Text.Json.Serialization;

namespace GotorzProjectMain.Services.APIs.HotelAPIs.DTOs;

// top-level DTO matching the whole payload
public sealed record HotelSearchResultDto(
    [property: JsonPropertyName("data")]
    IReadOnlyList<HotelDto>? Data,

    [property: JsonPropertyName("meta")]
    MetaDto? Meta
);

// Hotel
public sealed record HotelDto(
    [property: JsonPropertyName("chainCode")] string? ChainCode,
    [property: JsonPropertyName("iataCode")] string? IataCode,
    [property: JsonPropertyName("dupeId")] long? DupeId,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("hotelId")] string? HotelId,
    [property: JsonPropertyName("geoCode")] GeoCodeDto? GeoCode,
    [property: JsonPropertyName("address")] AddressDto? Address,
    [property: JsonPropertyName("distance")] DistanceDto? Distance
);

public sealed record GeoCodeDto(
    [property: JsonPropertyName("latitude")] double? Latitude,
    [property: JsonPropertyName("longitude")] double? Longitude
);

public sealed record AddressDto(
    [property: JsonPropertyName("countryCode")] string? CountryCode
);

public sealed record DistanceDto(
    [property: JsonPropertyName("value")] double? Value,
    [property: JsonPropertyName("unit")] string? Unit
);

public sealed record MetaDto(
    [property: JsonPropertyName("count")] int? Count,
    [property: JsonPropertyName("links")] LinksDto? Links
);

public sealed record LinksDto(
    [property: JsonPropertyName("self")] string? Self
);