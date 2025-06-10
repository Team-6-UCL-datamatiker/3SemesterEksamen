using Humanizer;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace GotorzProjectMain.Services;

public class VRNotifierService : IVRNotifierService
{
    // Multicast delegate:
    // The event keyword is what turns a delegate into a multicast delegate:
    // An internal list of all methods (matching the Func<Task> signature) that want to be notified when something happens.
    // Normal 1:1 delegate would just be public Func<Task>? OnChanged;
    public event Func<Task>? OnChanged;

    public async Task NotifyChangedAsync()
    {
        if (OnChanged is not null) // If there are subscribers
		{
            var handlers = OnChanged.GetInvocationList(); //Get all the individual delegates(handlers) subscribed.

			// Iterates through each handler and awaits their execution
			foreach (Func<Task> handler in handlers) // This runs all subscribed methods asynchronously and sequentially.
			{
                // Run each delegate asynchronously 
                await handler(); 
            }
        }
    }
}
