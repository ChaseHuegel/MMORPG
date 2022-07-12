using System.Numerics;

using Mmorpg.Data;
using Mmorpg.Server.Control;
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

        protected override void OnHealthChanged(HealthChangeEventArgs e)
        {
            Chat.Broadcast($"{e.Source.Name} {e.Cause.ToString().ToLower()} {Name} for {Math.Abs(e.Amount)} {e.Type.ToString().ToLower()}.", ChatChannel.Combat);
            
            HasUpdated = true;
            if (!IsAngry)
            {
                IsAngry = true;
                Target = e.Source;
                Chat.Broadcast($"{e.Source.Name} makes {Name} angry!", ChatChannel.Combat);
            }
        }

        protected override void OnDeath(HealthChangeEventArgs e)
        {
            if (e.Source != null)
                Chat.Broadcast($"{e.Source.Name} killed {Name}.", ChatChannel.Local);
            else
                Chat.Broadcast($"{Name} died.", ChatChannel.Local);
        }

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
                    Chat.Broadcast($"{Name} stops chasing {Target.Name}.", ChatChannel.Combat);
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
                        Target.Damage(1, EffectType.Bludgeoning, HealthChangeCause.Attacked, this);
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
