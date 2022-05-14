using UnityEngine;

namespace Kuhpik.Extensions
{
    public static class MathExtentions
    {
        // https://math.stackexchange.com/questions/754130/find-what-percent-x-is-between-two-numbers
        public static float GetPercentOfValueInRange(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }

        // https://docs.unity3d.com/2019.3/Documentation/Manual/DirectionDistanceFromOneObjectToAnother.html
        public static Vector3 GetDirection(this Transform from, Transform to)
        {
            return to.position - from.position;
        }

        public static Vector3 GetDirectionNormalized(this Transform from, Transform to)
        {
            return GetDirection(from, to).normalized;
        }

        public static Vector3 GetDirectionIgnoreY(this Transform from, Transform to)
        {
            var direction = GetDirection(from, to);
            direction.y = 0;
            return direction;
        }

        public static Vector3 GetDirectionNormalizedIgnoreY(this Transform from, Transform to)
        {
            return GetDirectionIgnoreY(from, to).normalized;
        }
    }
}
