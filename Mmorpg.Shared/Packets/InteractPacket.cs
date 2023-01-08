using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Shared.Packets
{
    [Packet(RequiresSession = true, Reliable = true)]
    public class InteractPacket : Packet
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
