using Unity.Mathematics;

namespace Utility
{
    public static class float2Extensions
    {
        public static float2 Rotate(this float2 v, float angleInDegrees)
        {
            float radians = math.radians(angleInDegrees);
            float sin = math.sin(radians);
            float cos = math.cos(radians);

            float x = v.x;
            float y = v.y;
            v.x = cos * x - sin * y;
            v.y = sin * x + cos * y;
            return v;
        }
    }
}