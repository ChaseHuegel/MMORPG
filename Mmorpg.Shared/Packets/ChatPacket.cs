using Needlefish;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Packets
{
    [Packet]
    public class ChatPacket : Packet
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
