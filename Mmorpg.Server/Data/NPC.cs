using System.Numerics;
using System;
using Mmorpg.Data;
using Mmorpg.Packets;

namespace Mmorpg.Server.Data
{
    public class NPC : LivingEntity
    {
        private static Random Random = new Random();

        private float Timer;

        private Vector3? m_Position;
        private Vector3 Position { 
            get => m_Position ?? (m_Position = new Vector3(X, Y, Z)).Value;
            set => m_Position = value;
        }

        public GameServer Server;

        public bool HasUpdated { get; private set; }

        public NPC()
        {
            Moving = true;
            Heading = Random.NextSingle() * 360f;
        }

        public override void Tick(float deltaTime)
        {
            HasUpdated = false;
            Timer += deltaTime;
            if (Timer >= 1f)
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
                    }
                });
            }
        }
    }
}
