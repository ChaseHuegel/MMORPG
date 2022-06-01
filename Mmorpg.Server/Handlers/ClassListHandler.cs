using MMORPG.Server;
using MMORPG.Server.Util;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Enums;
using Mmorpg.Packets;
using MMORPG.Shared.Util;

namespace Mmorpg.Server.Handlers
{
    public static class ClassListHandler
    {
        [ServerPacketHandler]
        public static void OnClassListHandlerServer(NetServer server, ClassListPacket packet, NetEventArgs e)
        {
            packet.ClassNames = Characters.Classes.Select(x => x.Name).ToArray();
            server.Send(packet, e.EndPoint);

            Console.WriteLine($"[{e.EndPoint}] requested class list");
        }
    }
}
