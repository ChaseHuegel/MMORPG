using MMORPG.Server;
using MMORPG.Server.Util;

using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;
using Mmorpg.Enums;
using Mmorpg.Packets;
using Mmorpg.Server.Data;

namespace Mmorpg.Server.Handlers
{
    public static class JoinWorldHandler
    {
        [ServerPacketHandler]
        public static void OnJoinWorldPacketServer(NetServer server, JoinWorldPacket packet, NetEventArgs e)
        {
            JoinWorldFlags flags = JoinWorldFlags.None;

            //  Verify the endpoint is logged into an account and capture that account
            if (!GameServer.Instance.Logins.TryGetValue(e.EndPoint, out string username))
                flags |= JoinWorldFlags.NotLoggedIn;

            Character character = ServerCharacters.GetCharacterList(username)[packet.Slot-1];
            if (character == null)
                flags |= JoinWorldFlags.JoinFailed;

            //  Verify the endpoint isn't already logged into a character
            if (GameServer.Instance.Players.ContainsKey(e.EndPoint))
                flags |= JoinWorldFlags.JoinFailed;

            packet.Flags = (int)flags;
            server.Send(packet, e.EndPoint);

            if (flags == JoinWorldFlags.None)
            {
                GameServer.Instance.Players.TryAdd(e.EndPoint, character);
                Console.WriteLine($"[{e.EndPoint}] is entering the world as [{character.Name}] [slot: {packet.Slot}]");
            }
            else
            {
                Console.WriteLine($"[{e.EndPoint}] tried to enter the world as [{character?.Name}] [slot: {packet.Slot}]: {flags}");
            }
        }
    }
}
