using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Enums;
using Mmorpg.Packets;
using System;

namespace Mmorpg.Client.Handlers
{
    public static class JoinWorldHandler
    {
        [ClientPacketHandler]
        public static void OnJoinWorldPacketClient(NetClient client, JoinWorldPacket packet, NetEventArgs e)
        {
            JoinWorldFlags flags = (JoinWorldFlags)packet.Flags;

            if (flags == JoinWorldFlags.None)
            {
                Console.WriteLine($"Entering the world with character [{packet.Slot}]");
                client.Handshake();
            }
            else
            {
                Console.WriteLine($"Failed to enter the world with character [{packet.Slot}]: {flags}");
            }
        }
    }
}
