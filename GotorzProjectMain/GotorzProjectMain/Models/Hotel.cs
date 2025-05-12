namespace GotorzProjectMain.Models;

// RECORDS:
// Properties bliver implicit sat til { get; init; } (kan dog ændres til set;). Betyder at værdien kun kan sættes ved instantiering og derefter er den readonly
// Giver mening i tilfælde som når man får info in fra Json og vil være sikker på at man aldrig kommer til at overskrive den rigtige/oprindelige data.
// Mere info om records: https://365ucl.sharepoint.com/:i:/r/sites/datamatiker-online-dmoof24-amg/Delte%20dokumenter/Team%2006/Projekter/3%20Semester/Kilder/Vidensdeling/Screenshot%202025-05-10%20144332.png?csf=1&web=1&e=A4esuo

public sealed record Hotel
(
    string? Name,
    string? IataCode,
    int? DuplicateId,
    string? HotelId,
    string? CountryCode,
    IReadOnlyList<string>? Amenities,
    int? Rating,
    DateTime? LastUpdated,
    GeoCode? Location,
    Distance? Distance
)
{
    public IReadOnlyList<HotelOffer>? Offers { get; set; }
};

public sealed record GeoCode(
    double? Latitude,
    double? Longitude
    );

public sealed record Distance(
    double? Value,
    string? Unit
    );

// Essential information about a hotel offer
public sealed record HotelOffer(
    string? OfferId,
    string? CheckInDate, // Or DateTime, if you parse it
    string? CheckOutDate, // Or DateTime, if you parse it
    string? RateCode,
    string? BoardType,
    RoomInfo? Room,
    GuestInfo? Guests,
    PriceInfo? Price,
    PolicySummary? Policies
);

// Simplified room information for the Hotel model
public sealed record RoomInfo(
    string? Type,
    string? Category, // e.g., "STANDARD_ROOM", "DELUXE_ROOM"
    int? Beds,
    string? BedType, // e.g., "DOUBLE", "KING"
    string? DescriptionText
);

// Simplified guest information
public sealed record GuestInfo(
    int? Adults
);

// Simplified price information
public sealed record PriceInfo(
    string? Currency,
    string? TotalAmount, // Or decimal
    string? AverageNightlyAmount // Or decimal
);

// Simplified policy summary
public sealed record PolicySummary(
    string? PaymentType, // e.g., "prepay", "guarantee"
    string? Refundability, // e.g., "NON_REFUNDABLE", "REFUNDABLE_UP_TO_DEADLINE"
    string? CancellationDeadline // Or DateTimeOffset
);