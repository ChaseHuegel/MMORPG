using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Packets
{
    [Packet(RequiresSession = false, Reliable = true)]
    public class LoginPacket : Packet
    {
        public string Username;

        public string Password;

        //  TODO this should be AccountFlags when packets support enums
        public int Flags;
    }
}
