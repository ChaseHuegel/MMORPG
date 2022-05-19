using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Networking.Interfaces;

namespace Mmorpg.Packets
{
    [Packet]
    public struct ChatPacket : ISerializedPacket
    {
        public int Sender;

        public string Message;
    }
}
