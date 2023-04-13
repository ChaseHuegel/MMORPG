using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace MMO.Bridge.Packets;

[Packet(RequiresSession = true, Reliable = true)]
public class ChatPacket : Packet
{
    public int Sender;

    public int Target;

    public string? Message;

    //  TODO this should be an enum when they are supported
    public int Channel;

    public string? Error;

    public ChatPacket() { }

    public ChatPacket(int sender, int target, string? message, int channel, string? error = null)
    {
        Sender = sender;
        Target = target;
        Message = message;
        Channel = channel;
        Error = error;
    }
}
