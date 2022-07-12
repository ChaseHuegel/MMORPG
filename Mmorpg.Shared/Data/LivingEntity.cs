using System;
using System.Numerics;
using Mmorpg.Shared.Enums;
using Mmorpg.Shared.Util;
using Swordfish.Library.Extensions;
using Swordfish.Library.Util;

namespace Mmorpg.Data
{
    public class LivingEntity : Entity
    {
        public float Speed = 4.5f;
        public float Direction;
        public bool Jumped;
        public bool Moving;
        public int Race;
        public int Class;
        public int Health;
        public int MaxHealth;

        public static EventHandler<HealthChangeEventArgs> HealthChanged;
        public static EventHandler<HealthChangeEventArgs> Death;

        protected virtual void OnHealthChanged(HealthChangeEventArgs e) {}
        protected virtual void OnDeath(HealthChangeEventArgs e) {}

        public LivingEntity()
        {
            HealthChanged += OnHealthChanged;
            Death += OnDeath;
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            if (Moving)
            {
                Vector2 direction = MathUtils.DirectionFromDegrees(Heading);
                X += direction.X * Speed * deltaTime;
                Z += direction.Y * Speed * deltaTime;
            }
        }

        public void Heal(int amount, EffectType type, HealthChangeCause cause)
            => ModifyHealth(amount, type, cause, null);

        public void Heal(int amount, EffectType type, HealthChangeCause cause, LivingEntity healer)
            => ModifyHealth(amount, type, cause, healer);

        public void Damage(int amount, EffectType type, HealthChangeCause cause)
            => ModifyHealth(-amount, type, cause, null);

        public void Damage(int amount, EffectType type, HealthChangeCause cause, LivingEntity attacker)
            => ModifyHealth(-amount, type, cause, attacker);

        private void ModifyHealth(int amount, EffectType type, HealthChangeCause cause, LivingEntity source)
        {
            HealthChangeEventArgs args = new HealthChangeEventArgs {
                Amount = amount,
                Type = type,
                Cause = cause,
                Source = source,
                Target = this
            };

            if (!HealthChanged.TryInvoke(this, args))
                return;

            Health = MathS.Clamp(Health + amount, 0, MaxHealth);

            if (Health == 0 && !Death.TryInvoke(this, args))
                Health = 1;
        }

        private void OnHealthChanged(object sender, HealthChangeEventArgs e)
        {
            if (sender == this)
                OnHealthChanged(e);
        }

        private void OnDeath(object sender, HealthChangeEventArgs e)
        {
            if (sender == this)
                OnDeath(e);
        }
    }
}
