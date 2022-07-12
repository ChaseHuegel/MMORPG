using System;
using Mmorpg.Packets;
using Mmorpg.Shared.Enums;
using Swordfish.Library.Networking;

namespace Mmorpg.Server.Control
{
    public static class Chat
    {
        public static void Broadcast(string message, ChatChannel channel)
        {
            GameServer.Instance.Broadcast(new ChatPacket {
                Channel = (int)channel,
                Message = message
            });

            Console.WriteLine($"[SERVER] [{channel}]: {message}");
        }

        public static void Send(NetSession session, string message, ChatChannel channel)
        {
            GameServer.Instance.Send(new ChatPacket {
                Channel = (int)channel,
                Message = message
            }, session);

            Console.WriteLine($"[SERVER] [{channel}]->{session}: {message}");
        }

        public static void SendAndBroadcast(NetSession session, string message, string broadcast, ChatChannel channel)
        {
            GameServer.Instance.BroadcastExcept(new ChatPacket {
                Channel = (int)channel,
                Message = broadcast
            }, session);

            GameServer.Instance.Send(new ChatPacket {
                Channel = (int)channel,
                Message = message
            }, session);

            Console.WriteLine($"[SERVER] [{channel}]->{session}: {broadcast}");
        }
    }
}
