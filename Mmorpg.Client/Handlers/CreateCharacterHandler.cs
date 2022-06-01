using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Types;
using Mmorpg.Data;
using Mmorpg.Enums;
using Mmorpg.Packets;
using System;
using MMORPG.Shared.Util;
using Mmorpg.Shared.Data;

namespace Mmorpg.Client.Handlers
{
    public static class CreateCharacterHandler
    {
        [ClientPacketHandler]
        public static void OnCreateCharacterClient(NetClient client, CreateCharacterPacket packet, NetEventArgs e)
        {
            CharacterRace chosenRace = Characters.GetRace(packet.Race);
            CharacterClass chosenClass = Characters.GetClass(packet.Class);

            CreateCharacterFlags flags = (CreateCharacterFlags)packet.Flags;

            if (flags == CreateCharacterFlags.None)
            {
                Console.WriteLine($"Created character [{packet.Name}, {chosenRace} {chosenClass}]");
            }
            else
            {
                Console.WriteLine($"Failed to create character [{packet.Name}, {chosenRace} {chosenClass}]: {flags}");
            }
        }
    }
}
