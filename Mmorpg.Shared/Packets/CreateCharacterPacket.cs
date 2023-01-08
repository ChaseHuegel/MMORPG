using Needlefish;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Packets
{
    [Packet(RequiresSession = false, Reliable = true)]
    public class CreateCharacterPacket : Packet
    {
        public string Name;

        public int Race;

        public int Class;

        //  TODO should be an enum when packets support them
        public int Flags;
    }
}
