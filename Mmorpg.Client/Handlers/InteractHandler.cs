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
                if (packet.Source == client.Session.ID)
                    Console.WriteLine($"You interacted with {packet.Target}: [{action}:{packet.Value}].");
                else
                    Console.WriteLine($"{packet.Source} interacts with {packet.Target}: [{action}:{packet.Value}].");
            }
            else
            {
                Console.WriteLine($"Interaction [{action}:{packet.Value}] with {packet.Target} failed: {flags}");
            }
        }
    }
}
