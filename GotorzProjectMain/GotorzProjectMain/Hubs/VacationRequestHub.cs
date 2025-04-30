using Microsoft.AspNetCore.SignalR;

namespace GotorzProjectMain.Hubs;

public class VacationRequestHub : Hub
{
    public async Task SendVacationRequest()
    {
        Console.WriteLine($"[INBOUND] Hub received vacation request.");

        await Clients.All.SendAsync("ReceiveVacationRequest");

        Console.WriteLine($"[OUTBOUND] Hub broadcasted vacation request.");
    }
}