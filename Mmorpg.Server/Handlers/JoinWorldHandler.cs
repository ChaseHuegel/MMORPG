using MMORPG.Server;
using MMORPG.Server.Util;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Enums;
using Mmorpg.Packets;

namespace Mmorpg.Server.Handlers
{
    public static class JoinWorldHandler
    {
        [ServerPacketHandler]
        public static void OnJoinWorldPacketServer(NetServer server, JoinWorldPacket packet, NetEventArgs e)
        {
            JoinWorldFlags flags = JoinWorldFlags.None;

            //  Verify the endpoint is logged into an account and capture that account
            if (!GameServer.Instance.Logins.TryGetValue(e.EndPoint, out string username))
                flags |= JoinWorldFlags.NotLoggedIn;

            string characterName = Characters.GetCharacterList(username)[packet.Slot];

            packet.Flags = (int)flags;
            server.Send(packet, e.EndPoint);

            if (flags == JoinWorldFlags.None)
            {
                Console.WriteLine($"[{e.EndPoint}] is entering the world as [{characterName}]");
            }
            else
            {
                Console.WriteLine($"[{e.EndPoint}] tried to enter the world as [{characterName}]: {flags}");
            }
        }
    }
}
