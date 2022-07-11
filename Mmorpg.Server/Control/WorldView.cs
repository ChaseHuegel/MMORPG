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

        private Random Random { get; }

        private ConcurrentQueue<AbilityRequest> AbilityQueue = new ConcurrentQueue<AbilityRequest>();

        public WorldView(GameServer server)
        {
            Random = new Random();
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
            
            while (AbilityQueue.TryDequeue(out AbilityRequest abilityRequest))
            {
                switch (abilityRequest.Ability)
                {
                    case 0:
                        NPC npc = (NPC)abilityRequest.Target;
                        npc.HasUpdated = true;
                        npc.Health -= Random.Next(4) + 1;
                        npc.IsAngry = true;
                        npc.Target = abilityRequest.Source;
                        Console.WriteLine($"{npc.Name} aggros {abilityRequest.Source.Name}!");
                        break;
                }
            }

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
                        },
                        Health = player.Health
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
                Class = character.Class,
                Health = 10
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
                Class = newPlayer.Class,
                Health = newPlayer.Health
            };
            Server.BroadcastExcept(snapshot, session);

            State.Players.TryAdd(newPlayer.ID, newPlayer);
        }

        public void RemovePlayer(NetSession session)
        {
            State.Players.TryRemove(session.ID, out _);
        }

        public void QueueAbility(LivingEntity source, LivingEntity target, int ability)
        {
            AbilityQueue.Enqueue(new AbilityRequest {
                Source = source,
                Target = target,
                Ability = ability
            });
        }

        private class AbilityRequest
        {
            public LivingEntity Source;
            public LivingEntity Target;
            public int Ability;
        }
    }
}
