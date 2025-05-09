namespace GotorzProjectMain.Models;

public class Hotel
{
    public string? Name { get; set; }
    public string? IataCode { get; set; }
    public int? DuplicateId { get; set; }
    public string? HotelId { get; set; }
    public string? CountryCode { get; set; }
    public IReadOnlyList<string>? Amenities { get; set; }
    public int? Rating { get; set; }
    public DateTime? LastUpdated { get; set; }
    public GeoCode? Location { get; set; }
    public Distance? Distance { get; set; }
    public IReadOnlyList<HotelOffer>? Offers { get; set; }
}

public sealed record GeoCode(double? Latitude, double? Longitude);
public sealed record Distance(double? Value, string? Unit);

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