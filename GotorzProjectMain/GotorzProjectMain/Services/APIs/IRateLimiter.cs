namespace GotorzProjectMain.Services.APIs;

public interface IRateLimiter
{
    bool TryRequest();
}
