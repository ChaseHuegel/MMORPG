using Mmorpg.Data;
using Mmorpg.Shared.Packets;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Client.Handlers
{
    public static class EntityHandler
    {
        [ClientPacketHandler]
        public static void OnEntityClient(NetClient client, EntityPacket packet, NetEventArgs e)
        {
            LivingEntity entity = GameClient.Instance.Characters.GetOrAdd(packet.ID, (id) => new LivingEntity() { ID = id });
            
            entity.X = packet.X;
            entity.Y = packet.Y;
            entity.Z = packet.Z;
            entity.Heading = packet.Heading;
            entity.Size = packet.Size;
            entity.Name = packet.Name;
            entity.Label = packet.Label;
            entity.Title = packet.Title;
            entity.Description = packet.Description;
            entity.Speed = packet.Speed;
            entity.Direction = packet.Direction;
            entity.Jumped = packet.Jumped;
            entity.Moving = packet.Moving;
            entity.Race = packet.Race;
            entity.Class = packet.Class;
            entity.Health = packet.Health;
        }
    }
}
