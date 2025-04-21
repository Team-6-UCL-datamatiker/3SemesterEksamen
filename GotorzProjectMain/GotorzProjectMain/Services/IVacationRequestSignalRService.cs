namespace GotorzProjectMain.Services
{
	public interface IVacationRequestSignalRService
	{
		Task InitializeAsync();
		Task SendVacationRequestAsync();
		bool HasMissedUpdate { get; }
		event Func<Task>? OnVacationRequestReceived;
		void ClearMissedUpdate();
	}
}
