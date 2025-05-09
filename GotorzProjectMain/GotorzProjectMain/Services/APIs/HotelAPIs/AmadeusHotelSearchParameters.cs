namespace GotorzProjectMain.Services.APIs.HotelAPIs;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

/// Encapsulates all query parameters accepted by the Amadeus
/// /shopping/hotel-offers (getMultiHotelOffers) endpoint so the UI can bind
/// directly to this object. Every field is optional except "HotelIds"/>; 

public class AmadeusHotelSearchParameters
{
    /// **Required.** List of 8‑character Amadeus property codes that identify the
    /// hotels to search (e.g. "TILONCHR").
    [JsonPropertyName("hotelIds")]
    [Required]
    public List<string> HotelIds { get; set; } = new();

    /// Number of adult guests per room (1‑9).
    [JsonPropertyName("adults")]
    [Range(1, 9, ErrorMessage = "Must be between 1 and 9")]
    public int Adults { get; set; } = 1;

    /// Check‑in date (hotel local) in **YYYY‑MM‑DD**. Must be ≥ today.  Defaults to
    /// today if omitted.
    [JsonPropertyName("checkInDate")]
    [Required]
    public DateOnly CheckInDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    /// Check‑out date (hotel local) in **YYYY‑MM‑DD**. Must be ≥ <see cref="CheckInDate"/> + 1.
    /// Defaults to *checkInDate + 1* if omitted.
    [JsonPropertyName("checkOutDate")]
    [Required]
    public DateOnly CheckOutDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(7));

    /// ISO‑3166‑1 country code representing the traveller’s country of residence.
    [JsonPropertyName("countryOfResidence")]
    public string? CountryOfResidence { get; set; }

    /// Number of rooms requested (1‑9).
    [JsonPropertyName("roomQuantity")]
    [Range(1, 9, ErrorMessage = "Must be between 1 and 50")]
    public int? RoomQuantity { get; set; }

    /// Price‑per‑night filter (e.g. "200-300", "-300", "100-").  Requires that
    /// Currency is also supplied.
    [JsonPropertyName("priceRange")]
    public string? PriceRange { get; set; }

    /// ISO‑4217 currency code (e.g. "USD").  If unsupported by a property, that
    /// hotel’s local currency will be returned instead.
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "EUR";

    /// Payment‑policy filter. "NONE" (default) shows all.  Other possible API
    /// values include "DEPOSIT", "FULL_PREPAY" etc.
    [JsonPropertyName("paymentPolicy")]
    public string? PaymentPolicy { get; set; }

    /// Meal plan filter: ROOM_ONLY, BREAKFAST, HALF_BOARD, FULL_BOARD,
    /// ALL_INCLUSIVE.
    [JsonPropertyName("boardType")]
    public string? BoardType { get; set; }

    /// When true, include sold‑out properties in the result set.  Default: false.
    [JsonPropertyName("includeClosed")]
    public bool? IncludeClosed { get; set; }

    /// When true, return only the cheapest offer per hotel.  Default: false.
    [JsonPropertyName("bestRateOnly")]
    public bool? BestRateOnly { get; set; }

    /// Two‑letter or BCP‑47 language code for descriptive texts (e.g. "fr",
    /// "fr-FR").  Falls back to English if unavailable.
    [JsonPropertyName("lang")]
    public string? Lang { get; set; }
}
