using System.Numerics;

using Mmorpg.Shared.Util;

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
    }
}
