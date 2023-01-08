using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Shared.Packets
{
    [Packet(RequiresSession = true, Reliable = true)]
    public class RequestWorldStatePacket : Packet
    {

    }
}
