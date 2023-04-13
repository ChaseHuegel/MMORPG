namespace MMO.Bridge.Events;

public delegate Task AsyncEventHandler<TEventArgs>(object? sender, TEventArgs args);

public static class AsyncEventExtensions
{
    public static async Task InvokeAsync<TEventArgs>(
        this AsyncEventHandler<TEventArgs> handler,
        object? sender,
        TEventArgs args
    )
    {
        if (handler == null)
            return;

        var listeners = handler.GetInvocationList().Cast<Func<object?, TEventArgs, Task>>();
        foreach (var listener in listeners)
            await listener(sender, args);
    }
}
