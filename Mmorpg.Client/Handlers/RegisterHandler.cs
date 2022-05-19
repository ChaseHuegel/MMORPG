using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Enums;
using Mmorpg.Packets;

namespace Mmorpg.Client.Handlers
{
    public static class RegisterHandler
    {
        [ClientPacketHandler]
        public static void OnRegisterClient(NetClient client, RegisterPacket packet, NetEventArgs e)
        {
            RegisterFlags flags = (RegisterFlags)packet.Flags;

            if (flags == RegisterFlags.None)
                Console.WriteLine($"Account [{packet.Username}, {packet.Email}] registered!");
            else
                Console.WriteLine($"Failed to register account [{packet.Username}, {packet.Email}]: {flags}");
        }
    }
}
