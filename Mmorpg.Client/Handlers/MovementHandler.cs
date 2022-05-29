using Mmorpg.Data;
using Mmorpg.Shared.Packets;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Client.Handlers
{
    public static class MovementHandler
    {
        [ClientPacketHandler]
        public static void OnMovementClient(NetClient client, MovementPacket packet, NetEventArgs e)
        {
            //  Update the entity if it exists
            if (GameClient.Instance.Characters.TryGetValue(packet.ID, out LivingEntity character))
            {
                character.Heading = packet.Heading;
                character.Direction = packet.Direction;
                character.Jumped = packet.State[0];
                character.Moving = packet.State[1];
            }
        }
    }
}
