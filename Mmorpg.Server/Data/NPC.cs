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
            Speed = 4.5f;
            Heading = (Random.NextSingle() * 360f) - 180f;
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            HasUpdated = false;

            Timer += deltaTime;
            if (Timer >= 1f)
            {
                if (Random.NextSingle() > 0.5f)
                {
                    Heading = (Random.NextSingle() * 360f) - 180f;
                    Direction = Heading;
                    HasUpdated = true;
                }
                
                Timer = 0f;
            }

            if (Moving)
            {
                float[] direction = DirectionFromDegrees(Heading);

                X += direction[0] * Speed * deltaTime * 1f;
                Z += direction[1] * Speed * deltaTime * 1f;
            }

            if (HasUpdated)
            {
                Server.Broadcast(new EntityPacket {
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

        private const double DegToRad = Math.PI/180d;

        private static float[] DirectionFromDegrees(float degrees)
        {
            float[] coords = new float[2];
            double radians = degrees * DegToRad;

            coords[0] = (float) Math.Cos(radians);
            coords[1] = (float) Math.Sin(radians);

            return coords;
        }

        private static float[] RotatePosition(float x, float y, double degrees)
        {
            float[] coords = new float[2];
            double radians = degrees * DegToRad;

            double ca = Math.Cos(radians);
            double sa = Math.Sin(radians);

            coords[0] = (float)(ca *x - sa*y);
            coords[1] = (float)(sa *x + ca*y);

            return coords;
        }
    }
}
