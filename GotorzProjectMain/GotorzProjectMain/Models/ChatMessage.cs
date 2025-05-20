using System.ComponentModel.DataAnnotations;

namespace GotorzProjectMain.Models
{
	public class ChatMessage
	{
		public int MessageId { get; set; }
		public string SenderId { get; set; }
		public string Username { get; set; } = string.Empty;
		public string Message { get; set; } = string.Empty;
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
	}
}
