using MMO.Bridge.Types;
using Swordfish.Library.Networking;
using Swordfish.Library.IO;
using Swordfish.Library.Collections;
using MMO.Client.Control;

namespace MMO.Client.Commands;

public class QuitCommand : Command
{
    private ClientController _clientController;
    private NetController _netController;

    public QuitCommand(ClientController clientController, NetController netController)
    {
        _clientController = clientController;
        _netController = netController;
    }

    public override string Option => "quit";
    public override string Description => "Exits the application.";
    public override string ArgumentsHint => "";

    protected override Task<CommandState> InvokeAsync(ReadOnlyQueue<string> args)
    {
        _netController.Disconnect();
        _clientController.Exit();
        return Task.FromResult(CommandState.Success);
    }
}
