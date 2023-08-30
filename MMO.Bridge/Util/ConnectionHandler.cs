namespace MMO.Bridge.Util;

public abstract class ConnectionHandler : IPipelineService
{
    public virtual PipelineState State { get; private set; }

    public async Task StartAsync()
    {
        State = PipelineState.Active;
        while (!IsConnected())
        {
            try
            {
                //  TODO introduce backoff behavior
                await TryConnectAsync();
            }
            catch
            {
                continue;
            }
        }

        State = PipelineState.Complete;
    }

    public Task ReconnectAsync()
    {
        Disconnect();
        return StartAsync();
    }

    public abstract bool IsConnected();

    protected abstract Task TryConnectAsync();

    protected abstract void Disconnect();
}