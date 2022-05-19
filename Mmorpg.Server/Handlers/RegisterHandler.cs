using MMORPG.Server.Util;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Enums;
using Mmorpg.Packets;

namespace Mmorpg.Server.Handlers
{
    public static class RegisterHandler
    {
        [ServerPacketHandler]
        public static void OnRegisterServer(NetServer server, RegisterPacket packet, NetEventArgs e)
        {
            RegisterFlags flags = Accounts.ValidateUsername(packet.Username) | Accounts.ValidatePassword(packet.Password) | Accounts.ValidateEmail(packet.Email);

            if (flags == RegisterFlags.None)
                Accounts.Register(packet.Username, packet.Password, packet.Email);

            packet.Flags = (int)flags;
            server.Send(packet, e.EndPoint);

            if (flags == RegisterFlags.None)
                Console.WriteLine($"[{e.EndPoint}] registered account [{packet.Username}, {packet.Email}]");
            else
                Console.WriteLine($"[{e.EndPoint}] tried to register account [{packet.Username}, {packet.Email}]: {flags}");
        }
    }
}
