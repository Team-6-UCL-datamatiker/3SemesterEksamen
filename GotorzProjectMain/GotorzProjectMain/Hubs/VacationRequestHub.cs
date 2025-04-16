using Microsoft.AspNetCore.SignalR;

namespace GotorzProjectMain.Hubs;

public class VacationRequestHub : Hub
{
    public async Task SendVacationRequest(int vacationId)
    {
        await Clients.All.SendAsync("ReceiveVacationRequest", vacationId);
    }
}