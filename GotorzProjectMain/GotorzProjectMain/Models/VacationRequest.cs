using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.Models
{
	public class VacationRequest
	{
		[Key]
		public int VacationRequestId { get; set; }
		public string Country { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int ChildrenAmount { get; set; }
		public int AdultsAmount { get; set; }
		public int RoomsAmount { get; set; }
		public string HotelRequest { get; set; }
		public string FlightRequest { get; set; }
		public string Misc { get; set; }
		public Status Status { get; set; }

		public string UserId { get; set; }
		public Customer Customer { get; set; }
	}

	public enum Status
	{
		PendingRequest,
		WatingApproval,
		BookingUnderway,
		Done
	}
}
