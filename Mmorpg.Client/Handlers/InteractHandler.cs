using System;

using Mmorpg.Shared.Enums;
using Mmorpg.Shared.Packets;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Client.Handlers
{
    public static class InteractHandler
    {
        [ClientPacketHandler]
        public static void OnInteractClient(NetClient client, InteractPacket packet, NetEventArgs e)
        {
            Interactions action = (Interactions)packet.Interaction;
            InteractFlags flags = (InteractFlags)packet.Flags;

            if (flags == InteractFlags.NONE)
            {
                Console.WriteLine($"Action [{action}:{packet.Value}] succeeded.");
            }
            else
            {
                Console.WriteLine($"Action [{action}:{packet.Value}] failed: {flags}");
            }
        }
    }
}
