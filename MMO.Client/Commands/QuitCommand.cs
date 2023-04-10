using MMO.Bridge.Commands;
using MMO.Bridge.Types;
using Swordfish.Library.Networking;

namespace MMO.Client.Commands;

public class QuitCommand : Command
{
    private Application _application;
    private NetController _netController;

    public QuitCommand(Application application, NetController netController)
    {
        _application = application;
        _netController = netController;
    }

    public override string Option => "quit";
    public override string Description => "Exits the application.";
    public override string ArgumentsHint => "";

    protected override Task<CommandCompletion> InvokeAsync(ReadOnlyQueue<string> args)
    {
        _netController.Disconnect();
        _application.Exit();
        return Task.FromResult(CommandCompletion.Success);
    }
}