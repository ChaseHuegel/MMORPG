using System;
using Mmorpg.Data;
using Mmorpg.Shared.Packets;
using Swordfish.Library.Networking;
using Swordfish.Library.Networking.Attributes;

namespace Mmorpg.Server.Handlers
{
    public static class RequestWorldStateHandler
    {
        [ServerPacketHandler]
        public static void OnRequestWorldStateServer(NetServer server, RequestWorldStatePacket packet, NetEventArgs e)
        {
            //  Send a snapshot of all players
            foreach (LivingEntity player in GameServer.Instance.WorldView.State.Players.Values)
            {
                server.Send(new EntityPacket {
                    ID = player.ID,
                    X = player.X,
                    Y = player.Y,
                    Z = player.Z,
                    Heading = player.Heading,
                    Size = player.Size,
                    Name = player.Name,
                    Label = player.Label,
                    Title = player.Title,
                    Description = player.Description,
                    Speed = player.Speed,
                    Direction = player.Direction,
                    Jumped = player.Jumped,
                    Moving = player.Moving,
                    Race = player.Race,
                    Class = player.Class,
                    Health = player.Health
                }, e.Session);
            }

            //  Send a snapshot of all npcs
            foreach (LivingEntity player in GameServer.Instance.WorldView.State.NPCs.Values)
            {
                server.Send(new EntityPacket {
                    ID = player.ID,
                    X = player.X,
                    Y = player.Y,
                    Z = player.Z,
                    Heading = player.Heading,
                    Size = player.Size,
                    Name = player.Name,
                    Label = player.Label,
                    Title = player.Title,
                    Description = player.Description,
                    Speed = player.Speed,
                    Direction = player.Direction,
                    Jumped = player.Jumped,
                    Moving = player.Moving,
                    Race = player.Race,
                    Class = player.Class,
                    Health = player.Health
                }, e.Session);
            }
        }
    }
}
