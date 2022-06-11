using System;

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
                float[] direction = DirectionFromDegrees(Heading);
                X += direction[0] * Speed * deltaTime;
                Z += direction[1] * Speed * deltaTime;
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
