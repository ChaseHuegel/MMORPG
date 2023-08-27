using MMO.Bridge.Packets;
using MMO.Bridge.Types;
using Swordfish.Library.Networking;
using Swordfish.Library.IO;
using Swordfish.Library.Collections;


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

    protected override Task<CommandState> InvokeAsync(ReadOnlyQueue<string> args)
    {
        string message = string.Join(' ', args.TakeAll());
        _netController.Broadcast(new ChatPacket(_netController.Session.ID, 0, message, (int)ChatChannel.General));

        return Task.FromResult(CommandState.Success);
    }
}
