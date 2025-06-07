using GotorzProjectMain.Data;
using GotorzProjectMain.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace GotorzProjectMain.Hubs
{
	public class ChatHub : Hub
	{
		private readonly ApplicationDbContext _context;

		public ChatHub(ApplicationDbContext context)
		{
			_context = context;
		}

		// Called when a user sends a new message
		public async Task SendMessage(string message, string userId, string username)
		{
			// Create new chat message entity
			ChatMessage chatMessage = new ChatMessage
			{
				SenderId = userId,
				Username = username,
				Message = message,
				Timestamp = DateTime.Now
			};

			// Store the message in the database
			_context.ChatMessages.Add(chatMessage);
			await _context.SaveChangesAsync();

			// Broadcast the message to all connected clients
			await Clients.All.SendAsync("ReceiveMessage", username, message, chatMessage.Timestamp);
		}

		// Retrieves the latest N chat messages (default = 50)
		public async Task<IEnumerable<ChatMessage>> GetRecentMessages(int count = 50)
		{
			return await _context.ChatMessages
							.OrderByDescending(m => m.Timestamp) // Get latest first
							.Take(count)
							.OrderBy(m => m.Timestamp)   // Then re-sort chronologically (oldest first)
							.ToListAsync();
		}

	}
}
