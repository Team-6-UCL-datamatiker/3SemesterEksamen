namespace GotorzProjectMain.Models;

public class HotelBooking
{
    public int HotelBookingId { get; set; }
    public string HotelName { get; set; }
    public string? Address { get; set; }
    public string? RoomDescription { get; set; }
    public double TotalPrice { get; set; }
    public string? BookingInformationLink { get; set; }
    public string? HotelLink { get; set; }
    public DateTime CheckinDate { get; set; }
    public DateTime CheckoutDate { get; set; }
    public string? Misc { get; set; }
    public int? VacationOfferId { get; set; }
}
