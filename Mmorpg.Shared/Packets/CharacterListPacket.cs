using Needlefish;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Packets
{
    [Packet(RequiresSession = false, Reliable = true)]
    public class CharacterListPacket : Packet
    {
        public string[] CharacterNames;

        //  TODO should be an enum when packets support them
        public int Flags;
    }
}
