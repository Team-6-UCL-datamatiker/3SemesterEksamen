using Microsoft.AspNetCore.SignalR;

namespace GotorzProjectMain.Hubs;

public class VacationRequestHub : Hub
{
    public async Task SendVacationRequest()
    {
		// Notify all connected clients about the vacation request
		await Clients.All.SendAsync("ReceiveVacationRequest");
    }
}