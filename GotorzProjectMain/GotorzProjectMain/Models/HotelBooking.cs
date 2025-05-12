using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GotorzProjectMain.Models
{
    public class HotelBooking
    {
        [Key]
        public int HotelBookingId { get; set; }

        public string HotelName { get; set; }

        public string Address { get; set; }

        public string RoomDescription { get; set; }

        public float Price { get; set; }

        public string BookingInformationLink { get; set; }

        public string HotelLink { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public string Misc { get; set; }

        [ForeignKey("VacationOffer")]
        public int VacationOfferId { get; set; }

        public VacationOffer VacationOffer { get; set; }
    }
}