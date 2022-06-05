using Mmorpg.Shared.Packets;
using MMORPG.Shared.Util;
using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Types;

namespace Mmorpg.Server.Handlers
{
    public static class CharacterCreationRulesHandler
    {
        [ServerPacketHandler]
        public static void OnClassListHandlerServer(NetServer server, CharacterCreationRulesPacket packet, NetEventArgs e)
        {
            packet.RaceClassMask = Characters.RaceClassCombinations.Select(mask => mask.bits).ToArray();
            packet.RaceNames = Characters.Races.Select(x => x.Name).ToArray();
            packet.ClassNames = Characters.Classes.Select(x => x.Name).ToArray();
            
            server.Send(packet, e.EndPoint);
            Console.WriteLine($"[{e.EndPoint}] requested character creation rules.");
        }
    }
}
