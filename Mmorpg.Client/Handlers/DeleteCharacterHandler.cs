using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Enums;
using Mmorpg.Packets;
using System;

namespace Mmorpg.Client.Handlers
{
    public static class DeleteCharacterHandler
    {
        [ClientPacketHandler]
        public static void OnDeleteCharacterClient(NetClient client, DeleteCharacterPacket packet, NetEventArgs e)
        {
            DeleteCharacterFlags flags = (DeleteCharacterFlags)packet.Flags;

            if (flags == DeleteCharacterFlags.None)
            {
                Console.WriteLine($"Deleted character slot [{packet.Slot}]");
            }
            else
            {
                Console.WriteLine($"Failed to delete character [{packet.Slot}]: {flags}");
            }
        }
    }
}
