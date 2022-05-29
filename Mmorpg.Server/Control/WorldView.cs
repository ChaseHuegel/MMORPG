using System.Collections.Concurrent;

using Mmorpg.Data;
using Mmorpg.Packets;
using Mmorpg.Server.Data;
using Swordfish.Library.Diagnostics;
using Swordfish.Library.Networking;

namespace Mmorpg.Server.Control
{
    public class WorldView
    {
        private const float LongTickRate = 1f;

        private float LongTickTime;

        private GameServer Server;

        public ConcurrentDictionary<int, LivingEntity> Players = new ConcurrentDictionary<int, LivingEntity>();
        public ConcurrentDictionary<int, NPC> NPCs = new ConcurrentDictionary<int, NPC>();

        private Random Random = new Random();

        public WorldView(GameServer server)
        {
            Server = server;

            for (int i = 0; i < 50; i++)
            {
                int id = Random.Next(100000, int.MaxValue);
                NPCs.TryAdd(i, new NPC {
                    Name = $"NPC{id}",
                    ID = id,
                    Server = Server
                });
            }
        }

        public void Tick(float deltaTime)
        {
            LongTickTime += deltaTime;
            if (LongTickTime >= LongTickRate)
                LongTickTime = 0f;

            foreach (NPC npc in NPCs.Values)
            {
                //  Tick each entity
                npc.Tick(deltaTime);
            }

            foreach (LivingEntity player in Players.Values)
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
            LivingEntity livingEntity = new LivingEntity {
                ID = session.ID,
                Name = character.Name,
                Race = character.Race,
                Class = character.Class
            };

            //  Send a snapshot of the new player to all other players
            EntitySnapshotPacket snapshot = new EntitySnapshotPacket {
                ID = livingEntity.ID,
                X = livingEntity.X,
                Y = livingEntity.Y,
                Z = livingEntity.Z,
                Heading = livingEntity.Heading,
                Speed = livingEntity.Speed,
                Direction = livingEntity.Direction,
                State = {
                    [0] = livingEntity.Jumped,
                    [1] = livingEntity.Moving
                }
            };
            Server.BroadcastExcept(snapshot, session);

            //  Send a snapshot of all entities to the new player
            foreach (LivingEntity entity in Players.Values)
            {
                Server.Send(new EntitySnapshotPacket {
                    ID = entity.ID,
                    X = entity.X,
                    Y = entity.Y,
                    Z = entity.Z,
                    Heading = entity.Heading,
                    Speed = entity.Speed,
                    Direction = entity.Direction,
                    State = {
                        [0] = entity.Jumped,
                        [1] = entity.Moving
                    }
                }, session);
            }

            Players.TryAdd(livingEntity.ID, livingEntity);
        }
    }
}
