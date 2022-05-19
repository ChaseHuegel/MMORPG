using MMORPG.Server;
using MMORPG.Server.Util;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Enums;
using Mmorpg.Packets;

namespace Mmorpg.Server.Handlers
{
    public static class CharacterListHandler
    {
        [ServerPacketHandler]
        public static void OnCharacterListPacketServer(NetServer server, CharacterListPacket packet, NetEventArgs e)
        {
            AccountFlags flags = AccountFlags.None;

            if (!GameServer.Instance.Logins.TryGetValue(e.EndPoint, out string username))
                flags |= AccountFlags.UsernameIncorrect;

            if (flags == AccountFlags.None)
                packet.CharacterNames = Characters.GetCharacterList(username);
            
            packet.Flags = (int)flags;
            server.Send(packet, e.EndPoint);
            
            if (flags == AccountFlags.None)
            {
                Console.WriteLine($"[{e.EndPoint}] requested character list for [{username}]");
            }
            else
            {
                Console.WriteLine($"[{e.EndPoint}] failed to request character list for [{username}]: {flags}");
            }
        }
    }
}
