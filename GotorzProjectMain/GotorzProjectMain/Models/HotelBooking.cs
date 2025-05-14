using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GotorzProjectMain.Models
{
    public class HotelBooking
    {
        public int HotelBookingId { get; set; }
        public string? HotelName { get; set; }
        public int HotelRating { get; set; }
        public string? Address { get; set; }
        public int? Adults { get; set; }
        public string? RoomDescription { get; set; }        
        // Food stuff
        public string? BoardType { get; set; }
        public float Price { get; set; }
        public string? Currency {  get; set; }
        public string? BookingInformationLink { get; set; }
        public string? HotelLink { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string? Misc { get; set; }
        public int? VacationOfferId { get; set; }
        public VacationOffer? VacationOffer { get; set; }
    }
}