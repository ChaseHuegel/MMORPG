using MMORPG.Server;
using MMORPG.Server.Util;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Enums;
using Mmorpg.Packets;
using MMORPG.Shared.Util;

namespace Mmorpg.Server.Handlers
{
    public static class RaceListHandler
    {
        [ServerPacketHandler]
        public static void OnRaceListClient(NetServer server, RaceListPacket packet, NetEventArgs e)
        {
            packet.RaceNames = Characters.Races.Select(x => x.Name).ToArray();
            server.Send(packet, e.EndPoint);

            Console.WriteLine($"[{e.EndPoint}] requested race list");
        }
    }
}
