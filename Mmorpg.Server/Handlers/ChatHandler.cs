using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Packets;

namespace Mmorpg.Server.Handlers
{
    public static class ChatHandler
    {
        [ServerPacketHandler]
        public static void OnChatReceived(NetServer server, ChatPacket packet, NetEventArgs e)
        {
            //  The server has authority over identifying the sender
            packet.Sender = e.Session.ID;
            server.Broadcast(packet);

            Console.WriteLine($"[CHAT] {packet.Sender}: {packet.Message}");
        }
    }
}
