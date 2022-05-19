using MMORPG.Server;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Data;
using Mmorpg.Packets;

namespace Mmorpg.Server.Handlers
{
    public static class MovementHandler
    {
        [ServerPacketHandler]
        public static void OnMovementServer(NetServer server, MovementPacket packet, NetEventArgs e)
        {
            //  Update the entity if it exists
            if (GameServer.Instance.Characters.TryGetValue(packet.ID, out LivingEntity character))
            {
                character.Heading = packet.Heading;
                character.Direction = packet.Direction;
                character.Jumped = packet.State[0];
                character.Moving = packet.State[1];

                EntityPacket snapshot = new EntityPacket {
                    ID = character.ID,
                    X = character.X,
                    Y = character.Y,
                    Z = character.Z,
                    Heading = character.Heading,
                    Speed = character.Speed,
                    Direction = character.Direction,
                    State = {
                        [0] = character.Jumped,
                        [1] = character.Moving
                    }
                };

                server.BroadcastExcept(snapshot, e.Session);
            }
        }
    }
}
