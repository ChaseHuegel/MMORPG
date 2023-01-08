using Needlefish;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Packets
{
    [Packet(RequiresSession = false, Reliable = true)]
    public class JoinWorldPacket : Packet
    {
        public int Slot;

        //  TODO should be an enum when packets support them
        public int Flags;
    }
}
