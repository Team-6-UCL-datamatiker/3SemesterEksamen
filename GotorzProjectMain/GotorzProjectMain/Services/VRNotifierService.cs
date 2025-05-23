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
            // OnChanged.GetInvocationList() are direct references/pointers to the instances with subscribed methods
            // In other words a list of delegates pointing to specifc method code. 
            var handlers = OnChanged.GetInvocationList();
            foreach (Func<Task> handler in handlers)
            {
                // Run each delegate asynchronously 
                await handler();
            }
        }
    }
}

// Extra info
//
// Multicast delegate:
//
// The event keyword is what turns a delegate into a multicast delegate—internally,
// a list of handlers—enabling you to use += and -= to add or remove as many methods as you want.
// Without event, a Func<Task> is just a variable that can hold one delegate (one method).
// With event, event Func<Task> is a built-in “subscription list” (multicast delegate) that can notify any number of subscribers.
// Think of event as the difference between one phone line (delegate) and a conference call(event with a list of delegates).
// That’s why event is fundamental for patterns where you want multiple things to react to a change.
// It’s what makes all the classic C# event/listener infrastructure work.
//
// OnChanged.GetInvocationList():
//
// Gives you an array of all the methods(delegates/handlers) currently subscribed to the event.
// Each one is a Delegate object pointing to a specific method attached via +=.
// So when you call GetInvocationList(), you can loop through and call each one individually
// — giving you control over how (and in what order, or in parallel) you invoke them.
//
// They’re called from the service—the one that raises the event—not from the original class where the handler (method) is defined.
// When the service calls NotifyChangedAsync() and iterates over OnChanged.GetInvocationList(),
// it invokes each handler from inside the service.

// Each handler runs in the context of whatever component/class defined it, but the call comes from the notifier service.
// If you inspect OnChanged.GetInvocationList() in the service, each entry is a Delegate object that “remembers” two things for each handler:
// The target object (the actual instance where the method lives—e.g., your Index component)
// The method pointer(the specific method to call—e.g., Index.HandleChanged)
// E.g.:
// Target: GotorzProjectMain.Pages.VacationRequests.Index
// Method: System.Threading.Tasks.Task HandleChanged()
// 
// When the service runs a delegate:
//
// The runtime takes the stored delegate.
// It calls the method directly on the original object (this) using the pointer.
// 
// Methods after compilation:
// After compilation, the code for a method is a stand-alone, uniquely-addressable piece of code that all objects use.
// The only thing that changes is which object the method runs against.
// Normally only the instance that holds a private method knows the pointer for that code, BUT
// if the pointer is EXPLICITELY GIVEN THROUGH A DELEGATE, then it can be executed from elsewhere.
// What is enforced is then: “Hey, runtime, please run [this method] on [this object] now.”
//
// API analogy:
//
// A private method is like a locked-down API endpoint — normally, only code inside the class has access.
// When you create and pass a delegate to that private method(via event subscription), you’re effectively handing out an access key(a pointer).
// The key isn’t just an address—it’s the precise credentials: “You can now invoke this code, on this instance, even if you couldn’t before.”
// The service (or anyone holding the delegate) doesn’t care about the access modifier—it now has the direct means to run that code, as if it was invited past the velvet rope.
// So, in our event subscription example:
// The method is privately "registered".
// The event holds a reference (the key/the pointer) to run it, bypassing visibility restrictions, because you explicitly allowed it at subscription time.
// If you never subscribe your private method, nobody can ever call it.
// If you do subscribe, whoever you gave the delegate to can always call it — even if it’s private.