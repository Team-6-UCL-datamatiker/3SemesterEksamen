using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GotorzProjectMain.Models
{
	public class Layover
	{
        public int LayoverId { get; set; }
        public int FlightRouteId { get; set; }
		public string Name { get; set; }
		public int Duration { get; set; }  // minutes
		public string DurationDisplay
		  => $"{Duration / 60}h {Duration % 60}m";
	}
}
