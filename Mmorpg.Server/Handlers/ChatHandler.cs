using Mmorpg.Data;
using Mmorpg.Packets;
using Mmorpg.Shared.Enums;

using Swordfish.Library.Extensions;
using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Server.Handlers
{
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
            if (channel == ChatChannel.Tell)
            {
                //  Validate the target
                NetSession target = server.GetSessions().ElementAtOrDefault(packet.Target);
        
                if (target != null)
                {
                    server.Send(packet, e.Session);
                    server.Send(packet, target);
                    Console.WriteLine($"[CHAT] [{channel}] {e.Session.ID}->{target.ID}: {packet.Message}");
                }
                else if (packet.Target == 0)
                {
                    server.Send(new ChatPacket {
                        Channel = (int)ChatChannel.System,
                        Flags = (int)ChatFlags.NoTarget
                    }, e.Session);
                    Console.WriteLine($"[CHAT] [{channel}] {packet.Sender}->NONE: {packet.Message}");
                }
                else
                {
                    server.Send(new ChatPacket {
                        Channel = (int)ChatChannel.System,
                        Flags = (int)ChatFlags.TargetOffline
                    }, e.Session);
                    Console.WriteLine($"[CHAT] [{channel}] {packet.Sender}->OFFLINE: {packet.Message}");
                }
            }
            else
            {
                server.Broadcast(packet);
                Console.WriteLine($"[CHAT] [{channel}] {packet.Sender}: {packet.Message}");
            }
        }
    }
}
