using System;
using MMO.Bridge.Packets;
using MMO.Bridge.Types;
using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace MMO.Client.Handlers;

public static class ChatHandler
{
    [ClientPacketHandler]
    public static void OnChatReceived(NetClient client, ChatPacket packet, NetEventArgs e)
    {
        if (string.IsNullOrEmpty(packet.Error))
            Console.WriteLine($"[CHAT] [{(ChatChannel)packet.Channel}] {packet.Sender}: {packet.Message}");
        else
            Console.WriteLine($"[CHAT] [{(ChatChannel)packet.Channel}] {packet.Error}");
    }
}