using MMO.Bridge.Events;

namespace MMO.Client;

public class Application
{
    private volatile int _exitCode = -1;

    public AsyncEventHandler<EventArgs>? UpdateAsync;
    public AsyncEventHandler<EventArgs>? ExitedAsync;

    public EventHandler<EventArgs>? Update;
    public EventHandler<EventArgs>? Exited;

    public void Exit(int code = 0)
    {
        _exitCode = code;
    }

    public async Task<int> RunAsync()
    {
        while (_exitCode < 0)
        {
            if (Update != null)
            {
                Update?.Invoke(this, EventArgs.Empty);
                await UpdateAsync!.InvokeAsync(this, EventArgs.Empty);
            }
            else
            {
                await Task.Delay(10);
            }
        }

        Exited?.Invoke(this, EventArgs.Empty);
        await ExitedAsync!.InvokeAsync(this, EventArgs.Empty);
        return _exitCode;
    }
}