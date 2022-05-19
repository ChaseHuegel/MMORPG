using MMORPG.Server;
using MMORPG.Server.Util;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Types;
using Mmorpg.Data;
using Mmorpg.Enums;
using Mmorpg.Packets;

namespace Mmorpg.Server.Handlers
{
    public static class CreateCharacterHandler
    {
        [ServerPacketHandler]
        public static void OnCreateCharacterServer(NetServer server, CreateCharacterPacket packet, NetEventArgs e)
        {
            DynamicEnumValue chosenRace = CharacterRaces.Get(packet.Race);
            DynamicEnumValue chosenClass = CharacterClasses.Get(packet.Class);

            CreateCharacterFlags flags = Characters.ValidateAndCleanName(packet.Name, out string cleanName);// | Characters.ValidateRaceClass(chosenRace, chosenClass);

            //  Verify the endpoint is logged into an account and capture that account
            if (!GameServer.Instance.Logins.TryGetValue(e.EndPoint, out string username))
                flags |= CreateCharacterFlags.NotLoggedIn;

            if (!Characters.TryGetOpenSlot(username, out int slot))
                flags |= CreateCharacterFlags.NoOpenSlot;

            if (flags == CreateCharacterFlags.None)
                Characters.CreateCharacter(cleanName, chosenRace, chosenClass, username, slot);

            packet.Name = cleanName;
            packet.Flags = (int)flags;
            server.Send(packet, e.EndPoint);

            if (flags == CreateCharacterFlags.None)
            {
                Console.WriteLine($"[{e.EndPoint}] created character [{packet.Name}, {chosenRace} {chosenClass}]");
            }
            else
            {
                Console.WriteLine($"[{e.EndPoint}] tried to create character [{packet.Name}, {chosenRace} {chosenClass}]: {flags}");
            }
        }
    }
}
