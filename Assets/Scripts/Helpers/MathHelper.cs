using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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

    public static float CalculatePolygonArea(List<Vector3> list)
    {
        Vector3 result = Vector3.zero;
        for (int p = list.Count - 1, q = 0; q < list.Count; p = q++)
        {
            result += Vector3.Cross(list[q], list[p]);
        }
        result *= 0.5f;
        return result.magnitude;
    }


}
