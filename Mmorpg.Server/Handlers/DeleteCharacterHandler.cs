using MMORPG.Server;
using MMORPG.Server.Util;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Enums;
using Mmorpg.Packets;

namespace Mmorpg.Server.Handlers
{
    public static class DeleteCharacterHandler
    {
        [ServerPacketHandler]
        public static void OnDeleteCharacterServer(NetServer server, DeleteCharacterPacket packet, NetEventArgs e)
        {
            DeleteCharacterFlags flags = DeleteCharacterFlags.None;

            //  Verify the endpoint is logged into an account and capture that account
            if (!GameServer.Instance.Logins.TryGetValue(e.EndPoint, out string username))
                flags |= DeleteCharacterFlags.NotLoggedIn;

            if (flags == DeleteCharacterFlags.None)
                Characters.DeleteCharacter(username, packet.Slot);

            packet.Flags = (int)flags;
            server.Send(packet, e.EndPoint);

            if (flags == DeleteCharacterFlags.None)
            {
                Console.WriteLine($"[{e.EndPoint}] deleted character slot [{packet.Slot}]");
            }
            else
            {
                Console.WriteLine($"[{e.EndPoint}] tried to delete character slot [{packet.Slot}]: {flags}");
            }
        }
    }
}
