using System.Numerics;

using Mmorpg.Data;
using Mmorpg.Shared.Enums;
using Mmorpg.Shared.Packets;
using Mmorpg.Shared.Util;

namespace Mmorpg.Server.Data
{
    public class NPC : LivingEntity
    {
        private static Random Random = new Random();

        private float Timer;

        public GameServer Server;

        public bool HasUpdated;

        public LivingEntity Target;

        public bool IsAngry;

        public bool IsDead;

        public bool DoesWander;
        public NPC Wander(bool value) { DoesWander = value; return this; }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            //  TODO handle death properly instead of disabling the AI
            if (Health <= 0)
            {
                if (!IsDead)
                {
                    IsDead = true;
                    Moving = false;
                    Snapshot();
                }

                return;
            }

            if ((Timer += deltaTime) >= 1f)
                Timer = 0f;
            
            if (DoesWander && Timer == 0f)
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
            }

            if (IsAngry && Target != null)
            {
                Vector3 pos = new Vector3(X, Y, Z);
                Vector3 targetPos = new Vector3(Target.X, Target.Y, Target.Z);
                float distanceToTarget = MathUtils.DistanceUnsquared(targetPos, pos);

                if (distanceToTarget > 100)
                {
                    //  Deaggro
                    Console.WriteLine($"{Name} deaggros {Target.Name}.");
                    IsAngry = false;
                    Moving = false;
                    Target = null;
                    HasUpdated = true;
                }
                else if (Timer == 0f)
                {
                    //  Try to attack
                    bool inRange = distanceToTarget <= 6;
                    if (inRange)
                    {
                        Target.Health -= 1;
                        Console.WriteLine($"{Name} attacks {Target.Name}!");
                        Server.Broadcast(new InteractPacket {
                            Source = ID,
                            Target = Target.ID,
                            Interaction = (int)Interactions.ABILITY,
                            Value = 0
                        });
                        TryUpdateHeading();
                    }

                    //  If our moving state will be changing, flag as updated
                    if (Moving == inRange)
                        HasUpdated = true;

                    //  Try to chase
                    Moving = !inRange;
                    if (Moving)
                        TryUpdateHeading();

                    void TryUpdateHeading()
                    {
                        float newHeading = pos.Direction(targetPos).ToVector2().ToDegrees();
                        if (newHeading != Heading)
                            HasUpdated = true;
                        Heading = Direction = newHeading;
                    }
                }
            }

            if (HasUpdated)
            {
                HasUpdated = false;
                Snapshot();
            }
        }

        private void Snapshot()
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
                },
                Health = Health
            });
        }
    }
}
