using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Enums;
using Mmorpg.Packets;

namespace Mmorpg.Client.Handlers
{
    public static class LoginHandler
    {
        [ClientPacketHandler]
        public static void OnLoginClient(NetClient client, LoginPacket packet, NetEventArgs e)
        {
            AccountFlags flags = (AccountFlags)packet.Flags;

            if (flags == AccountFlags.None)
            {
                Console.WriteLine($"Logged into account [{packet.Username}]");
            }
            else
            {
                Console.WriteLine($"Failed to login to account [{packet.Username}]: {flags}");
            }
        }
    }
}
