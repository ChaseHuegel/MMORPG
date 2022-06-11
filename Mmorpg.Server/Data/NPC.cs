using System.Numerics;
using System;
using Mmorpg.Data;
using Mmorpg.Shared.Packets;

namespace Mmorpg.Server.Data
{
    public class NPC : LivingEntity
    {
        private static Random Random = new Random();

        private float Timer;

        public GameServer Server;

        public bool HasUpdated;

        public bool DoesWander;
        public NPC Wander(bool value) { DoesWander = value; return this; }

        public override void Tick(float deltaTime)
        {
            Timer += deltaTime;
            if (Timer >= 1f && DoesWander)
            {
                if (Random.NextSingle() > 0.5f)
                {
                    Heading = Random.NextSingle() * 360f;
                    Direction = Heading;
                    Moving = true;
                }
                else
                {
                    Moving = false;
                }
                
                HasUpdated = true;
                Timer = 0f;
            }

            base.Tick(deltaTime);

            if (HasUpdated)
            {
                HasUpdated = false;
                Server.Broadcast(new EntitySnapshotPacket {
                    ID = ID,
                    X = X,
                    Y = Y,
                    Z = Z,
                    Heading = Heading,
                    Speed = Speed,
                    Direction = Direction,
                    State = {
                        [0] = Jumped,
                        [1] = Moving
                    },
                    Health = Health
                });
            }
        }
    }
}
