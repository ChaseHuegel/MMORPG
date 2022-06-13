using System;
using System.Numerics;

namespace Mmorpg.Shared.Util
{
    //  TODO these will need to be cleaned up and moved into the library
    public static class MathUtils
    {
        public const double DegreesToRadians = Math.PI/180d;
        public const double RadiansToDegrees = 180d/Math.PI;

        public static float DistanceUnsquared(Vector3 firstPosition, Vector3 secondPosition)
        {
            return (firstPosition.X - secondPosition.X) * (firstPosition.X - secondPosition.X) +
                    (firstPosition.Y - secondPosition.Y) * (firstPosition.Y - secondPosition.Y) +
                    (firstPosition.Z - secondPosition.Z) * (firstPosition.Z - secondPosition.Z);
        }

        public static Vector3 Direction(this Vector3 from, Vector3 to) => to - from;

        public static Vector2 ToVector2(this Vector3 vec3) => new Vector2(vec3.X, vec3.Z);

        public static Vector2 DirectionFromDegrees(float degrees)
        {
            Vector2 coords = new Vector2();
            double radians = degrees * DegreesToRadians;

            coords.X = (float) Math.Cos(radians);
            coords.Y = (float) Math.Sin(radians);

            return coords;
        }

        public static float ToDegrees(this Vector2 vec2)
        {
            return (float)(Math.Atan2(vec2.Y, vec2.X) * RadiansToDegrees);
        }

        public static Vector2 Rotate(this Vector2 vec2, double degrees)
        {
            Vector2 coords = new Vector2();
            double radians = degrees * DegreesToRadians;

            double ca = Math.Cos(radians);
            double sa = Math.Sin(radians);

            coords.X = (float)(ca *vec2.X - sa*vec2.Y);
            coords.Y = (float)(sa *vec2.X + ca*vec2.Y);

            return coords;
        }
    }
}
