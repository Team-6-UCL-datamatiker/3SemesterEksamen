using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace GotorzProjectMain.Services;

public class VacationRequestSignalRService : IAsyncDisposable
{
    private readonly NavigationManager _navigationManager;
    private HubConnection? _hubConnection;
    private bool _isConnected = false;

    public event Func<Task>? OnVacationRequestReceived;
    
    private bool _missedUpdate = false;

    public VacationRequestSignalRService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
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
            Console.WriteLine("📥 Signal received: ReceiveVacationRequest");

            if (OnVacationRequestReceived is null)
            {
                Console.WriteLine("⚠️ No handler attached — deferring.");
                _missedUpdate = true;
            }
            else
            {
                Console.WriteLine("🔥 Invoking handler.");
                await OnVacationRequestReceived.Invoke();
                Console.WriteLine("✅ Handler done.");
            }
        });

        try
        {
            await _hubConnection.StartAsync();
            _isConnected = true;
            Console.WriteLine("✅ SignalR connected (service).");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to start SignalR: {ex.Message}");
        }
    }

    public async Task SendVacationRequestAsync()
    {
        Console.WriteLine("📤 Attempting to send vacation request update via SignalR...");
    
        if (_hubConnection is not null)
        {
            Console.WriteLine($"📡 Connection state: {_hubConnection.State}");

            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.SendAsync("SendVacationRequest");
                Console.WriteLine("📤 SignalR message sent.");
            }
            else
            {
                Console.WriteLine("⚠️ SignalR hub connection is not in 'Connected' state.");
            }
        }
        else
        {
            Console.WriteLine("❌ hubConnection is null.");
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