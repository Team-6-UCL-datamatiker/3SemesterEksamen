namespace GotorzProjectMain.Models
{
	public class Flight
	{
		public string DepartureAirportCode { get; set; }
		public string DepartureAirportName { get; set; }
		public string ArrivalAirportCode { get; set; }
		public string ArrivalAirportName { get; set; }
		public DateTime DepartureTime { get; set; }
		public DateTime ArrivalTime { get; set; }
		public decimal Price { get; set; }
		public string Airline { get; set; }
	}
}
