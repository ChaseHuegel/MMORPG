using Mmorpg.Enums;
using Mmorpg.Packets;
using Mmorpg.Shared.Data;
using MMORPG.Server.Util;
using MMORPG.Shared.Util;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Server.Handlers
{
    public static class CreateCharacterHandler
    {
        [ServerPacketHandler]
        public static void OnCreateCharacterServer(NetServer server, CreateCharacterPacket packet, NetEventArgs e)
        {
            CharacterRace chosenRace = Characters.GetRace(packet.Race);
            CharacterClass chosenClass = Characters.GetClass(packet.Class);

            CreateCharacterFlags flags = ServerCharacters.ValidateAndCleanName(packet.Name, out string cleanName) | Characters.ValidateRaceClassCombination(chosenRace, chosenClass);

            //  Verify the endpoint is logged into an account and capture that account
            if (!GameServer.Instance.Logins.TryGetValue(e.EndPoint, out string username))
                flags |= CreateCharacterFlags.NotLoggedIn;

            if (!ServerCharacters.TryGetOpenSlot(username, out int slot))
                flags |= CreateCharacterFlags.NoOpenSlot;

            if (flags == CreateCharacterFlags.None)
                ServerCharacters.CreateCharacter(cleanName, chosenRace, chosenClass, username, slot);

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
