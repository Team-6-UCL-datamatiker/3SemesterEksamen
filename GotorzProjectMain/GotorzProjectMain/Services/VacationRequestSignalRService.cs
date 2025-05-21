using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;



namespace GotorzProjectMain.Services;

public class VacationRequestSignalRService : IAsyncDisposable
{
    private readonly NavigationManager _navigationManager;
    private readonly ILogger<VacationRequestSignalRService> _logger;
    private HubConnection? _hubConnection;
    private bool _isConnected = false;

    public event Func<Task>? OnVacationRequestReceived;
    
    private bool _missedUpdate = false;

    public VacationRequestSignalRService(NavigationManager navigationManager, ILogger<VacationRequestSignalRService> logger)
    {
        _navigationManager = navigationManager;
        _logger = logger;
	}

    public async Task InitializeAsync()
    {
        if (_isConnected) return;

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/vacationrequesthub"))
            .WithAutomaticReconnect()
            .Build();

		_hubConnection.On("ReceiveVacationRequest", async () =>
        {
            _logger.LogInformation("Signal received: ReceiveVacationRequest");

            if (OnVacationRequestReceived is null)
            {
                _missedUpdate = true;
            }
            else
            {
                await OnVacationRequestReceived.Invoke();
            }
        });

        try
        {
			// Start the connection
			await _hubConnection.StartAsync();
            _isConnected = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start SignalR connection.");
        }
    }
    
    public async Task SendVacationRequestAsync()
    {
        if (_hubConnection is not null && _hubConnection.State == HubConnectionState.Connected)
        {
            _logger.LogInformation("Sending vacation request to SignalR hub.");
			await _hubConnection.SendAsync("SendVacationRequest");
        }
        else
        {
			_logger.LogWarning("Cannot send vacation request. Hub connection is not established or is null.");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
            await _hubConnection.DisposeAsync();
    }
    public bool HasMissedUpdate => _missedUpdate;

    public void ClearMissedUpdate()
    {
        _missedUpdate = false;
    }
}