using System.Collections.Concurrent;

using Mmorpg.Data;
using Mmorpg.Packets;
using Mmorpg.Server.Data;
using Mmorpg.Shared.Packets;

using Swordfish.Library.Diagnostics;
using Swordfish.Library.Networking;

namespace Mmorpg.Server.Control
{
    public class WorldView
    {
        private const float LongTickRate = 1f;

        private float LongTickTime;

        private GameServer Server;

        public WorldState State { get; private set; }

        public WorldView(GameServer server)
        {
            Server = server;

            State = new WorldState();
            foreach (NPC npc in State.NPCs.Values)
            {
                npc.Server = Server;
            }
        }

        public void Tick(float deltaTime)
        {
            LongTickTime += deltaTime;
            if (LongTickTime >= LongTickRate)
                LongTickTime = 0f;

            foreach (NPC npc in State.NPCs.Values)
            {
                //  Tick each entity
                npc.Tick(deltaTime);
            }

            foreach (LivingEntity player in State.Players.Values)
            {
                //  Tick each player
                player.Tick(deltaTime);

                if (LongTickTime == 0f)
                {
                    //  Broadcast a snapshot of each player each long tick
                    Server.Broadcast(new EntitySnapshotPacket {
                        ID = player.ID,
                        X = player.X,
                        Y = player.Y,
                        Z = player.Z,
                        Heading = player.Heading,
                        Speed = player.Speed,
                        Direction = player.Direction,
                        State = {
                            [0] = player.Jumped,
                            [1] = player.Moving
                        }
                    });
                }
            }
        }

        public void AddPlayer(Character character, NetSession session)
        {
            LivingEntity newPlayer = new LivingEntity {
                ID = session.ID,
                Name = character.Name,
                Race = character.Race,
                Class = character.Class
            };

            //  Send a snapshot of the new player to all other players
            EntityPacket snapshot = new EntityPacket {
                ID = newPlayer.ID,
                X = newPlayer.X,
                Y = newPlayer.Y,
                Z = newPlayer.Z,
                Heading = newPlayer.Heading,
                Size = newPlayer.Size,
                Name = newPlayer.Name,
                Label = newPlayer.Label,
                Title = newPlayer.Title,
                Description = newPlayer.Description,
                Speed = newPlayer.Speed,
                Direction = newPlayer.Direction,
                Jumped = newPlayer.Jumped,
                Moving = newPlayer.Moving,
                Race = newPlayer.Race,
                Class = newPlayer.Class
            };
            Server.BroadcastExcept(snapshot, session);

            State.Players.TryAdd(newPlayer.ID, newPlayer);
        }
    }
}
