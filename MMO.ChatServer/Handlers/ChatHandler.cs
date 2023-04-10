using MMO.Bridge.Packets;
using MMO.Bridge.Types;
using Swordfish.Library.Extensions;
using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace MMO.ChatServer.Handlers;

public static class ChatHandler
{
    [ServerPacketHandler]
    public static void OnChatReceived(NetServer server, ChatPacket packet, NetEventArgs e)
    {
        //  The server has authority over identifying the sender
        packet.Sender = e.Session.ID;

        //  Curate the message
        packet.Message = packet.Message.TruncateUpTo(64).TrimEnd();

        ChatChannel channel = (ChatChannel)packet.Channel;
        if (packet.Target > 0)
        {
            //  Validate the target
            NetSession? target = server.GetSessions().ElementAtOrDefault(packet.Target);

            if (target != null)
            {
                server.Send(packet, e.Session);
                server.Send(packet, target);
                Console.WriteLine(
                    $"[CHAT] [{channel}] {e.Session.ID}->{target.ID}: {packet.Message}"
                );
            }
            else
            {
                server.Send(
                    new ChatPacket
                    {
                        Channel = (int)ChatChannel.System,
                        Error = "That user is not online."
                    },
                    e.Session
                );
                Console.WriteLine($"[CHAT] [{channel}] {packet.Sender}->OFFLINE: {packet.Message}");
            }
        }
        else
        {
            server.BroadcastExcept(packet, server.Session);
            Console.WriteLine($"[CHAT] [{channel}] {packet.Sender}: {packet.Message}");
        }
    }
}
