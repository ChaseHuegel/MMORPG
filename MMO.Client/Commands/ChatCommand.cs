using MMO.Bridge.Commands;
using MMO.Bridge.Packets;
using MMO.Bridge.Types;
using Swordfish.Library.Networking;

namespace MMO.Client.Commands;

public class ChatCommand : Command
{
    public override string Option => "say";
    public override string Description => "Send a chat message.";
    public override string ArgumentsHint => "<message>";

    private readonly NetController _netController;

    public ChatCommand(NetController netController)
    {
        _netController = netController;
    }

    protected override Task<CommandCompletion> InvokeAsync(ReadOnlyQueue<string> args)
    {
        string message = string.Join(' ', args.TakeAll());

        if (_netController != null)
            _netController.Broadcast(new ChatPacket(_netController.Session.ID, 0, message, 0));
        else
            Console.WriteLine("Say: " + message);

        return Task.FromResult(CommandCompletion.Success);
    }
}
