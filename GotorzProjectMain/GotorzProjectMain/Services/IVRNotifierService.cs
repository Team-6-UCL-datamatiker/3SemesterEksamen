namespace GotorzProjectMain.Services;

public interface IVRNotifierService
{
    public event Func<Task>? OnChanged;
    public Task NotifyChangedAsync();
}
