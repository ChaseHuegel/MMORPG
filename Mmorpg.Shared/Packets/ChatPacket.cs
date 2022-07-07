using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Networking.Interfaces;

namespace Mmorpg.Packets
{
    [Packet]
    public struct ChatPacket : ISerializedPacket
    {
        public int Sender;

        public int Target;

        public string Message;

        //  TODO this should be an enum when they are supported
        public int Channel;

        //  TODO this should be an enum when they are supported
        public int Flags;
    }
}
