using Microsoft.AspNetCore.SignalR;
using GotorzProjectMain.Hubs;

namespace GotorzProjectMain.Services;

public sealed class VacationRequestNotifier : IVacationRequestNotifier
{
    private readonly IHubContext<VacationRequestHub> _hub;

    public VacationRequestNotifier(IHubContext<VacationRequestHub> hub)
    {
        _hub = hub;
    }

    public Task BroadcastAsync()
    {
        return _hub.Clients.All.SendAsync("ReceiveVacationRequest");
    }
}
