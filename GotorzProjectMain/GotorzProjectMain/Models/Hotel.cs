namespace GotorzProjectMain.Models;

public class Hotel
{
    public string? Name { get; set; }
    // Tror jeg kan få City med også, men er først helt sikker, når der er inde.
    public string? City { get; set; }
    public string? IataCode { get; set; }
    public int? DuplicateId { get; set; }
    public string? HotelId { get; set; }
    public string? CountryCode { get; set; }
    public GeoCode? Location { get; set; }
    public Distance? Distance { get; set; }
}

public sealed record GeoCode(double? Latitude, double? Longitude);
public sealed record Distance(double? Value, string? Unit);