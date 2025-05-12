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

		public async Task SendMessage(string message, string userId, string username)
		{
			ChatMessage chatMessage = new ChatMessage();

			chatMessage.SenderId = userId;
			chatMessage.Username = username;
			chatMessage.Message = message;
			chatMessage.Timestamp = DateTime.Now;

			_context.ChatMessages.Add(chatMessage);
			await _context.SaveChangesAsync();

			// Notify all clients about the new message
			await Clients.All.SendAsync("ReceiveMessage", username, message, chatMessage.Timestamp);
		}

		public async Task<IEnumerable<ChatMessage>> GetRecentMessages(int count = 50)
		{
			return await _context.ChatMessages
							.OrderByDescending(m => m.Timestamp)
							.Take(count)
							.OrderBy(m => m.Timestamp)   // back to chronological
							.ToListAsync();
		}

	}
}
