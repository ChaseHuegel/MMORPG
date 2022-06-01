using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Networking.Interfaces;

namespace Mmorpg.Packets
{
    [Packet(RequiresSession = false)]
    public struct ClassListPacket : ISerializedPacket
    {
        public string[] ClassNames;

        //  TODO should be an enum when packets support them
        public int Flags;
    }
}
