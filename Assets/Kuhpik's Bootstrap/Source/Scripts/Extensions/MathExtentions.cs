namespace Kuhpik.Extensions
{
    public static class MathExtentions
    {
        // https://math.stackexchange.com/questions/754130/find-what-percent-x-is-between-two-numbers
        public static float GetPercentOfValueInRange(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }
    }
}
