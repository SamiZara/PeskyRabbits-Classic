using UnityEngine;
using System.Collections;
using System;

public class MathHelper
{
    public static float degreeBetween2Points(Vector3 p1, Vector3 p2)
    {
        float xDiff = p2.x - p1.x;
        float yDiff = p2.y - p1.y;
        return (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);
    }

    public static float distanceBetween2Points(Vector3 p1, Vector3 p2)
    {
        return Mathf.Sqrt(Mathf.Pow(p2.x - p1.x, 2) + Mathf.Pow(p2.y - p1.y, 2));
    }
}
