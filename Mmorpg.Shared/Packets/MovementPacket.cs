using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Networking.Interfaces;
using Swordfish.Library.Types;

namespace Mmorpg.Shared.Packets
{
    [Packet(RequiresSession = true, Ordered = true)]
    public struct MovementPacket : ISerializedPacket
    {
        public int ID;

        public float Heading;

        public float Direction;

        public MultiBool State;
    }
}
