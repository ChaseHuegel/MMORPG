using Needlefish;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Packets
{
    [Packet(RequiresSession = false)]
    public struct CreateCharacterPacket : IDataBody
    {
        public string Name;

        public int Race;

        public int Class;

        //  TODO should be an enum when packets support them
        public int Flags;
    }
}
