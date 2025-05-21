namespace GotorzProjectMain.Services
{
    public interface IVacationRequestNotifier
    {
        public Task BroadcastAsync();
    }
}