using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;

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

    public static T[][] FindCombinations<T>(T[] seq)
    {
        var powerSet = new T[1 << seq.Length][];
        powerSet[0] = new T[0]; // starting only with empty set
        for (var i = 0; i < seq.Length; i++)
        {
            var cur = seq[i];
            var count = 1 << i; // doubling list each time
            for (var j = 0; j < count; j++)
            {
                var source = powerSet[j];
                var destination = powerSet[count + j] = new T[source.Length + 1];
                for (var q = 0; q < source.Length; q++)
                    destination[q] = source[q];
                destination[source.Length] = cur;
            }
        }
        return powerSet;
    }

    public static List<List<int>> GetCombinationSet<T>(T[][] seq)
    {
        List<List<int>> combinations = new List<List<int>>();
        for (var i = 0; i < seq.Length; i++)
        {
            var line = new StringBuilder();
            for (var j = 0; j < seq[i].Length; j++)
            {
                line.AppendFormat("{0},", seq[i][j]);
            }
            if (line.ToString() != "")
            {    
                string temp = line.ToString().TrimEnd(',');
                string[] data = temp.Split(',');
                List<int> set = new List<int>();
                for (int h = 0; h < data.Length; h++)
                {
                    set.Add(int.Parse(data[h]));
                }
                combinations.Add(set);
            }
        }
        return combinations;
    }
}
