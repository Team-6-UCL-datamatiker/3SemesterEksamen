using System.Text.Json.Serialization;

namespace GotorzProjectMain.Services.APIs.HotelAPIs;

// LIST API DTO's
//-----------------------------------------------------------------------------------------------------------------------------------------

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
    [property: JsonPropertyName("lastUpdate")] DateTime? LastUpdate,
    [property: JsonPropertyName("iataCode")] string? IataCode,
    [property: JsonPropertyName("dupeId")] long? DupeId,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("hotelId")] string? HotelId,
    [property: JsonPropertyName("geoCode")] GeoCodeDto? GeoCode,
    [property: JsonPropertyName("address")] AddressDto? Address,
    [property: JsonPropertyName("distance")] DistanceDto? Distance,
    [property: JsonPropertyName("amenities")] IReadOnlyList<string>? Amenities,
    [property: JsonPropertyName("rating")] int? Rating
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

// SEARCH API DTO's
//-----------------------------------------------------------------------------------------------------------------------------------------

// Top-level DTO for the entire hotel offers response
public sealed record HotelOffersResponseDto(
    [property: JsonPropertyName("data")] IReadOnlyList<HotelOfferDataDto>? Data,
    [property: JsonPropertyName("dictionaries")] DictionariesDto? Dictionaries,
    [property: JsonPropertyName("warnings")] IReadOnlyList<WarningDto>? Warnings
);

// DTO for each item in the "data" array
public sealed record HotelOfferDataDto(
    [property: JsonPropertyName("type")] string? Type,
    [property: JsonPropertyName("hotel")] HotelOfferHotelDto? Hotel,
    [property: JsonPropertyName("available")] bool? Available,
    [property: JsonPropertyName("offers")] IReadOnlyList<OfferDto>? Offers,
    [property: JsonPropertyName("self")] string? Self
);

// DTO for the "hotel" object within an offer data item
public sealed record HotelOfferHotelDto(
    [property: JsonPropertyName("type")] string? Type,
    [property: JsonPropertyName("hotelId")] string? HotelId,
    [property: JsonPropertyName("chainCode")] string? ChainCode,
    [property: JsonPropertyName("dupeId")] string? DupeId,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("cityCode")] string? CityCode,
    [property: JsonPropertyName("latitude")] double? Latitude,
    [property: JsonPropertyName("longitude")] double? Longitude
);

// DTO for each "offer" object
public sealed record OfferDto(
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("checkInDate")] string? CheckInDate, // Consider DateTime
    [property: JsonPropertyName("checkOutDate")] string? CheckOutDate, // Consider DateTime
    [property: JsonPropertyName("rateCode")] string? RateCode,
    [property: JsonPropertyName("commission")] CommissionDto? Commission,
    [property: JsonPropertyName("boardType")] string? BoardType,
    [property: JsonPropertyName("room")] RoomDto? Room,
    [property: JsonPropertyName("guests")] GuestsDto? Guests,
    [property: JsonPropertyName("price")] PriceDto? Price,
    [property: JsonPropertyName("policies")] PoliciesDto? Policies,
    [property: JsonPropertyName("self")] string? Self
);

// DTO for the "commission" object
public sealed record CommissionDto(
    [property: JsonPropertyName("percentage")] string? Percentage // Consider decimal or double
);

// DTO for the "room" object
public sealed record RoomDto(
    [property: JsonPropertyName("type")] string? Type,
    [property: JsonPropertyName("typeEstimated")] TypeEstimatedDto? TypeEstimated,
    [property: JsonPropertyName("description")] RoomDescriptionDto? Description
);

// DTO for the "typeEstimated" object
public sealed record TypeEstimatedDto(
    [property: JsonPropertyName("category")] string? Category,
    [property: JsonPropertyName("beds")] int? Beds,
    [property: JsonPropertyName("bedType")] string? BedType
);

// DTO for the "description" object within "room"
public sealed record RoomDescriptionDto(
    [property: JsonPropertyName("text")] string? Text,
    [property: JsonPropertyName("lang")] string? Lang
);

// DTO for the "guests" object
public sealed record GuestsDto(
    [property: JsonPropertyName("adults")] int? Adults
);

// DTO for the "price" object
public sealed record PriceDto(
    [property: JsonPropertyName("currency")] string? Currency,
    [property: JsonPropertyName("total")] string? Total, // Consider decimal or double
    [property: JsonPropertyName("taxes")] IReadOnlyList<TaxDto>? Taxes,
    [property: JsonPropertyName("variations")] PriceVariationsDto? Variations
);

// DTO for each "tax" object
public sealed record TaxDto(
    [property: JsonPropertyName("code")] string? Code,
    [property: JsonPropertyName("percentage")] string? Percentage, // Consider decimal or double
    [property: JsonPropertyName("included")] bool? Included
);

// DTO for the "variations" object within "price"
public sealed record PriceVariationsDto(
    [property: JsonPropertyName("average")] AveragePriceDto? Average,
    [property: JsonPropertyName("changes")] IReadOnlyList<PriceChangeDto>? Changes
);

// DTO for the "average" price object
public sealed record AveragePriceDto(
    [property: JsonPropertyName("total")] string? Total // Consider decimal or double
);

// DTO for each "change" in price
public sealed record PriceChangeDto(
    [property: JsonPropertyName("startDate")] string? StartDate, // Consider DateTime
    [property: JsonPropertyName("endDate")] string? EndDate,     // Consider DateTime
    [property: JsonPropertyName("base")] string? Base         // Consider decimal or double
);

// DTO for the "policies" object
public sealed record PoliciesDto(
    [property: JsonPropertyName("cancellations")] IReadOnlyList<CancellationPolicyDto>? Cancellations,
    [property: JsonPropertyName("prepay")] PrepayPolicyDto? Prepay,
    [property: JsonPropertyName("guarantee")] GuaranteePolicyDto? Guarantee,
    [property: JsonPropertyName("paymentType")] string? PaymentType,
    [property: JsonPropertyName("refundable")] RefundablePolicyDto? Refundable
);

// DTO for each "cancellation" policy
public sealed record CancellationPolicyDto(
    [property: JsonPropertyName("deadline")] string? Deadline, // Consider DateTimeOffset for timezone
    [property: JsonPropertyName("policyType")] string? PolicyType
);

// DTO for the "prepay" policy
public sealed record PrepayPolicyDto(
    [property: JsonPropertyName("amount")] string? Amount, // Consider decimal or double
    [property: JsonPropertyName("acceptedPayments")] AcceptedPaymentsDto? AcceptedPayments
);

// DTO for the "guarantee" policy
public sealed record GuaranteePolicyDto(
    [property: JsonPropertyName("acceptedPayments")] AcceptedPaymentsDto? AcceptedPayments
);


// DTO for "acceptedPayments" (used by both prepay and guarantee)
public sealed record AcceptedPaymentsDto(
    [property: JsonPropertyName("creditCards")] IReadOnlyList<string>? CreditCards,
    [property: JsonPropertyName("methods")] IReadOnlyList<string>? Methods,
    [property: JsonPropertyName("creditCardPolicies")] IReadOnlyList<CreditCardPolicyDto>? CreditCardPolicies
);

// DTO for each "creditCardPolicy"
public sealed record CreditCardPolicyDto(
    [property: JsonPropertyName("vendorCode")] string? VendorCode
);

// DTO for the "refundable" policy
public sealed record RefundablePolicyDto(
    [property: JsonPropertyName("cancellationRefund")] string? CancellationRefund
);

// DTO for the "dictionaries" object
public sealed record DictionariesDto(
    [property: JsonPropertyName("currencyConversionLookupRates")]
    Dictionary<string, CurrencyConversionRateDto>? CurrencyConversionLookupRates // Using Dictionary for dynamic keys like "GBP"
);

// DTO for each currency conversion rate
public sealed record CurrencyConversionRateDto(
    [property: JsonPropertyName("rate")] string? Rate, // Consider decimal or double
    [property: JsonPropertyName("target")] string? Target,
    [property: JsonPropertyName("targetDecimalPlaces")] int? TargetDecimalPlaces
);

// DTO for each "warning" object
public sealed record WarningDto(
    [property: JsonPropertyName("code")] int? Code,
    [property: JsonPropertyName("title")] string? Title,
    [property: JsonPropertyName("detail")] string? Detail,
    [property: JsonPropertyName("source")] WarningSourceDto? Source
);

// DTO for the "source" object within a warning
public sealed record WarningSourceDto(
    [property: JsonPropertyName("parameter")] string? Parameter
);
