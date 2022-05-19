using MMORPG.Server;
using MMORPG.Server.Util;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Enums;
using Mmorpg.Packets;

namespace Mmorpg.Server.Handlers
{
    public static class LoginHandler
    {
        [ServerPacketHandler]
        public static void OnLoginServer(NetServer server, LoginPacket packet, NetEventArgs e)
        {
            AccountFlags flags = AccountFlags.None;

            if (!Accounts.VerifyUsername(packet.Username))
                flags |= AccountFlags.UsernameIncorrect;

            if (!Accounts.VerifyPassword(packet.Username, packet.Password))
                flags |= AccountFlags.PasswordIncorrect;
            
            if (flags == AccountFlags.None && !GameServer.Instance.Logins.TryAdd(e.EndPoint, packet.Username))
                flags |= AccountFlags.AlreadyLoggedIn;

            packet.Flags = (int)flags;
            server.Send(packet, e.EndPoint);

            if (flags == AccountFlags.None)
            {
                Console.WriteLine($"[{e.EndPoint}] logged into account [{packet.Username}]");
            }
            else
            {
                Console.WriteLine($"[{e.EndPoint}] tried to login to account [{packet.Username}]: {flags}");
            }
        }
    }
}
