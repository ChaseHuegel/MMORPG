using Needlefish;

using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Shared.Packets
{
    [Packet(RequiresSession = true)]
    public struct RequestWorldStatePacket : IDataBody
    {
        
    }
}
