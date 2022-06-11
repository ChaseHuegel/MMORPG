using System;
using System.Numerics;

namespace Mmorpg.Server.Util
{
    public static class MathUtils
    {
        //  TODO this should be replaced by a method in the library
        public static float DistanceUnsquared(Vector3 firstPosition, Vector3 secondPosition)
        {
            return (firstPosition.X - secondPosition.X) * (firstPosition.X - secondPosition.X) +
                    (firstPosition.Y - secondPosition.Y) * (firstPosition.Y - secondPosition.Y) +
                    (firstPosition.Z - secondPosition.Z) * (firstPosition.Z - secondPosition.Z);
        }
    }
}
