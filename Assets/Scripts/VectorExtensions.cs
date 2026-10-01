using UnityEngine;

public static class VectorExtensions
{
    public static bool ApproximatelyEqual(this Vector3 a, Vector3 b, float accuracy = 1e-3f)
    {
        return (a - b).sqrMagnitude <= accuracy * accuracy;
    }
}