using static System.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorUtils
{
    public static Vector3 RandomHorizontalUnitVector()
    {
        Vector2 vec2 = Random.insideUnitCircle.normalized;
        return new Vector3(vec2.x, 0, vec2.y);
    }
}
