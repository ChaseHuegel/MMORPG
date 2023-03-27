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
}
