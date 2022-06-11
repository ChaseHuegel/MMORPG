using Mmorpg.Data;
using Mmorpg.Packets;
using Mmorpg.Shared.Packets;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Server.Handlers
{
    public static class MovementHandler
    {
        [ServerPacketHandler]
        public static void OnMovementServer(NetServer server, MovementPacket packet, NetEventArgs e)
        {
            //  Update the player if it exists
            if (GameServer.Instance.WorldView.State.Players.TryGetValue(packet.ID, out LivingEntity player))
            {
                player.Heading = packet.Heading;
                player.Direction = packet.Direction;
                player.Jumped = packet.State[0];
                player.Moving = packet.State[1];

                EntitySnapshotPacket snapshot = new EntitySnapshotPacket {
                    ID = player.ID,
                    X = player.X,
                    Y = player.Y,
                    Z = player.Z,
                    Heading = player.Heading,
                    Speed = player.Speed,
                    Direction = player.Direction,
                    State = {
                        [0] = player.Jumped,
                        [1] = player.Moving
                    },
                    Health = player.Health
                };

                server.BroadcastExcept(snapshot, e.Session);
            }
        }
    }
}
