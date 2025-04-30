using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GotorzProjectMain.Models
{
	public class VacationRequest
	{
		[Key]
		public int VacationRequestId { get; set; }
		public string Country { get; set; }
		public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;

		public int ChildrenAmount { get; set; }

		public int AdultsAmount { get; set; }

		public int RoomsAmount { get; set; }

		public string HotelRequest { get; set; }
		public string FlightRequest { get; set; }
		public string Misc { get; set; }
		public RequestStatus Status { get; set; } = RequestStatus.PendingRequest;

		[ForeignKey("Customer")]
        public string UserId { get; set; }

		public Customer Customer { get; set; }

        [Timestamp]
		public byte[] RowVersion { get; set; } = new byte[8];
	}

	public enum RequestStatus
	{
		PendingRequest,
		WaitingApproval,
		BookingUnderway,
		Done
	}
}
