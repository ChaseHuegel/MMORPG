using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Packets;
using System;

namespace Mmorpg.Client.Handlers
{
    public static class ChatHandler
    {
        [ClientPacketHandler]
        public static void OnChatReceived(NetClient client, ChatPacket packet, NetEventArgs e)
        {
            Console.WriteLine($"[CHAT] {packet.Sender}: {packet.Message}");
        }
    }
}
