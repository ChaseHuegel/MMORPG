using Needlefish;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Types;

namespace Mmorpg.Shared.Packets
{
    [Packet(RequiresSession = true, Ordered = true)]
    public struct EntitySnapshotPacket : IDataBody
    {
        public int ID;
        
        public float X;

        public float Y;

        public float Z;

        public float Heading;

        public float Speed;

        public float Direction;

        public MultiBool State;

        public int Health;
    }
}
