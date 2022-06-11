using System.Diagnostics;
using System.Numerics;
using System;
using Mmorpg.Server.Data;
using Mmorpg.Server.Util;
using Mmorpg.Shared.Enums;
using Mmorpg.Shared.Packets;
using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Swordfish.Library.Util;
using Mmorpg.Data;
using Mmorpg.Server.Control;

namespace Mmorpg.Server.Handlers
{
    public static class InteractHandler
    {
        [ServerPacketHandler]
        public static void OnInteractServer(NetServer server, InteractPacket packet, NetEventArgs e)
        {
            Interactions action = (Interactions)packet.Interaction;

            switch (action)
            {
                case Interactions.ABILITY:
                    ProcessAbility(server, packet, e);
                    break;
            }
        }

        private static void ProcessAbility(NetServer server, InteractPacket packet, NetEventArgs e)
        {
            Interactions action = (Interactions)packet.Interaction;
            InteractFlags flags = InteractFlags.NONE;
            WorldView worldView = GameServer.Instance.WorldView;
            WorldState worldState = worldView.State;
            
            //  Verify there is a target
            if (!worldState.NPCs.TryGetValue(packet.TargetEntity, out NPC target))
                flags |= InteractFlags.NO_TARGET;
            
            //  Verify the target is in range
            if (worldState.Players.TryGetValue(e.Session.ID, out LivingEntity player) && target != null && MathUtils.DistanceUnsquared(new Vector3(player.X, player.Y, player.Z), new Vector3(target.X, target.Y, target.Z)) > 6)
                flags |= InteractFlags.TOO_FAR_AWAY;
            
            //  Verify the target is valid
            //  TODO this should check against alignment, faction, and group
            if (!(target is NPC))
                flags |= InteractFlags.INVALID_TARGET;
            
            packet.Flags = (int)flags;
            server.Send(packet, e.Session);
            if (flags == InteractFlags.NONE)
            {
                //  TODO Value should be used to attempt locating a registered ability
                //  TODO and validate that Player has access to it and it's ready to use (ie. off CD, not CCd, etc)
                worldView.QueueAbility(player, target, packet.Value);
                Console.WriteLine($"{player.Name} targets {target.Name} with {action}:{packet.Value}!");
            }
            else
            {
                Console.WriteLine($"{player?.Name.ToString() ?? e.Session.EndPoint.ToString()} failed {action}:{packet.Value} targeting {target.Name}: {flags}");
            }
        }
    }
}
