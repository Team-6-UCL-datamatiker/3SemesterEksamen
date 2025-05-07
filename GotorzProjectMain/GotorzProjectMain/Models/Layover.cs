namespace GotorzProjectMain.Models
{
	public class Layover
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public int Duration { get; set; }  // minutes

		public string DurationDisplay
		  => $"{Duration / 60}h {Duration % 60}m";
	}
}
