using Needlefish;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Packets
{
    [Packet(RequiresSession = false)]
    public struct LoginPacket : IDataBody
    {
        public string Username;

        public string Password;

        //  TODO this should be AccountFlags when packets support enums
        public int Flags;
    }
}
