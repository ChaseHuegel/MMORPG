using Needlefish;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Packets
{
    [Packet(RequiresSession = false)]
    public class RegisterPacket : Packet
    {
        public string Username;

        public string Password;

        public string Email;

        //  TODO this should be RegisterFlags when packets support enums
        public int Flags;
    }
}
