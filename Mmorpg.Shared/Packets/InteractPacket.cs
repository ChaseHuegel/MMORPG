using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Networking.Interfaces;

namespace Mmorpg.Shared.Packets
{
    [Packet(RequiresSession = true)]
    public struct InteractPacket : ISerializedPacket
    {
        public int Source;
        
        public int Target;

        public int Value;

        //  TODO this should be replaced with an enum when they are supported
        public int Interaction;

        //  TODO this should be replaced with an enum when they are supported
        public int Flags;
    }
}
