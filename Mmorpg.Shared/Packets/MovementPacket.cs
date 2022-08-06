using Needlefish.Types;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Shared.Packets
{
    [Packet(RequiresSession = true, Ordered = true)]
    public class MovementPacket : Packet
    {
        public int ID;

        public float Heading;

        public float Direction;

        public MultiBool State;
    }
}
