using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Networking.Interfaces;

namespace Mmorpg.Packets
{
    [Packet(RequiresSession = false)]
    public struct LoginPacket : ISerializedPacket
    {
        public string Username;

        public string Password;

        //  TODO this should be AccountFlags when packets support enums
        public int Flags;
    }
}
