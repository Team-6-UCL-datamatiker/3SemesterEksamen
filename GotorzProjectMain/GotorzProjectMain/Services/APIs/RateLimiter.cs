namespace GotorzProjectMain.Services.APIs;

// Simple in-memory rate limiter to restrict API requests per time window
public class RateLimiter : IRateLimiter
{
	private const int _maxRequests = 5;
	private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1);
	private readonly Queue<DateTime> _requestTimestamps = new();

	// Lidt overkill (kun en tråd ad gangen), men det skader heller ikke.
	// Hvis man ville skifte til serverwide singleton kontrol, ville det være nødvendigt.
	private readonly object _lock = new();

	public bool TryRequest()
	{
		lock (_lock)
		{
			// Fjern timestamps ældre end 1 min
			while (_requestTimestamps.Count > 0 && DateTime.UtcNow - _requestTimestamps.Peek() > _timeWindow)
			{
				_requestTimestamps.Dequeue();
			}

			// Allow request if under the limit
			if (_requestTimestamps.Count < _maxRequests)
			{
				_requestTimestamps.Enqueue(DateTime.UtcNow);
				return true;
			}

			// Deny if request limit reached
			return false;
		}
	}
}
