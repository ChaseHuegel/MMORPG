using Needlefish;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Shared.Packets
{
    [Packet]
    public struct EntityPacket : IDataBody
    {
        public int ID;

        public float X;

        public float Y;

        public float Z;

        public float Heading;

        public float Size;

        public string Name;

        public string Label;

        public string Title;

        public string Description;

        public float Speed;

        public float Direction;

        public bool Jumped;

        public bool Moving;

        public int Race;
        
        public int Class;

        public int Health;
    }
}
