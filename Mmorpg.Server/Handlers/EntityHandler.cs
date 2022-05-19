using MMORPG.Server;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Data;
using Mmorpg.Packets;

namespace Mmorpg.Server.Handlers
{
    public static class EntityHandler
    {
        [ServerPacketHandler]
        public static void OnEntitySnapshotServer(NetServer server, EntityPacket packet, NetEventArgs e)
        {
            //  Update the entity if it exists; otherwise create it.
            if (GameServer.Instance.Characters.TryGetValue(packet.ID, out LivingEntity character))
            {
                character.X = packet.X;
                character.Y = packet.Y;
                character.Z = packet.Z;
                character.Heading = packet.Heading;
                character.Speed = packet.Speed;
                character.Direction = packet.Direction;
                character.Jumped = packet.State[0];
                character.Moving = packet.State[1];
            }
            else
            {
                character = new LivingEntity {
                    ID = packet.ID,
                    X = packet.X,
                    Y = packet.Y,
                    Z = packet.Z,
                    Heading = packet.Heading,
                    Speed = packet.Speed,
                    Direction = packet.Direction,
                    Jumped = packet.State[0],
                    Moving = packet.State[1]
                };

                GameServer.Instance.Characters.TryAdd(packet.ID, character);
            }
        }
    }
}
