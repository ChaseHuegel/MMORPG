using System;

using Mmorpg.Packets;
using Mmorpg.Shared.Enums;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Client.Handlers
{
    public static class ChatHandler
    {
        [ClientPacketHandler]
        public static void OnChatReceived(NetClient client, ChatPacket packet, NetEventArgs e)
        {
            ChatFlags flags = (ChatFlags)packet.Flags;

            if (flags == ChatFlags.None)
                Console.WriteLine($"[CHAT] [{(ChatChannel)packet.Channel}] {packet.Sender}: {packet.Message}");
            else
                Console.WriteLine($"[CHAT] [{(ChatChannel)packet.Channel}] {flags}");
        }
    }
}
