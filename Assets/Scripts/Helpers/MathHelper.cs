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

    public static float CalculatePolygonArea(List<Vector2> list)
    {
        Vector3 result = Vector3.zero;
        for (int p = list.Count - 1, q = 0; q < list.Count; p = q++)
        {
            result += Vector3.Cross(list[q], list[p]);
        }
        result *= 0.5f;
        return result.magnitude;
    }

    public static bool IsPointInsideLine(Vector2 p1,Vector2 l1,Vector2 l2)
    {
        float degree = 0;
        degree = degreeBetween2Points(l1, l2);
        if (degree <= 0)
            degree += 360;
        if(degree == 90 || degree == 180 || degree == 270 || degree == 360)
        {
            if(l1.x == l2.x && p1.x == l1.x)
            {
                if((p1.y < l1.y && p1.y > l2.y) || (p1.y < l2.y && p1.y > l1.y))
                {
                    return true;
                }
            }
            else if (l1.y == l2.y && p1.y == l1.y)
            {
                if ((p1.x < l1.x && p1.x > l2.x) || (p1.x < l2.x && p1.x > l1.x))
                {
                    return true;
                }
            }
        }
        return false;
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

    public static bool ContainsPoint(Vector2[] polyPoints, Vector2 p)
    {
        var j = polyPoints.Length - 1;
        var inside = false;
        for (int i = 0; i < polyPoints.Length; j = i++)
        {
            if (((polyPoints[i].y <= p.y && p.y < polyPoints[j].y) || (polyPoints[j].y <= p.y && p.y < polyPoints[i].y)) &&
               (p.x < (polyPoints[j].x - polyPoints[i].x) * (p.y - polyPoints[i].y) / (polyPoints[j].y - polyPoints[i].y) + polyPoints[i].x))
                inside = !inside;
        }
        return inside;
    }

    public static List<List<Vector2>> FindPolygons(List<Vector2> points, Vector3 currentPoint, float currentDegree, List<List<Vector2>> polygonList, Vector2 firstPoint, List<Vector2> orderedList, bool isFirst)
    {
        List<Line2D> lineList = new List<Line2D>();
        for (int i = 0; i < orderedList.Count - 1; i++)
        {

            lineList.Add(new Line2D(orderedList[i], orderedList[i + 1]));
        }
        foreach (Line2D line in lineList)
        {
            foreach (Line2D line2 in lineList)
            {
                if (line != line2)
                {
                    if (line.intersectsLine(line2))
                    {
                        //Debug.Log("Intersection detected l1p1:" + line.P1 + " l1p2:" + line.P2 + " l2p1" + line2.P1 + " l2p2" + line2.P2);
                        return polygonList;
                    }
                }
            }
        }
        float degree = 0;
        degree = degreeBetween2Points(currentPoint, firstPoint);
        if (degree <= 0)
            degree += 360;
        //Debug.Log(degree + "," + currentDegree);
        //Debug.Log("p1:"+currentPoint+",p2:"+firstPoint+", "+degree+","+currentDegree);
        if ((Mathf.Abs(currentDegree - degree) == 90) || (Mathf.Abs(currentDegree - degree - 360) == 90) || (Mathf.Abs(currentDegree - degree + 360) == 90))
        {
            polygonList.Add(orderedList);
            //if (!isFirst)
              //  return polygonList;
        }
        float distance = float.MaxValue;
        Vector3 topPoint = Vector3.zero;
        Vector3 leftPoint = Vector3.zero;
        Vector3 bottomPoint = Vector3.zero;
        Vector3 rightPoint = Vector3.zero;
        List<Vector2> tempPointList = new List<Vector2>(points);
        tempPointList.Remove(currentPoint);
        List<Vector3> pointsAtTop = new List<Vector3>();
        List<Vector3> pointsAtLeft = new List<Vector3>();
        List<Vector3> pointsAtBottom = new List<Vector3>();
        List<Vector3> pointsAtRight = new List<Vector3>();
        foreach (Vector3 point in tempPointList)
        {

            degree = degreeBetween2Points(currentPoint, point);
            if (degree <= 0)
                degree += 360;
            //Debug.Log("degree:"+degree+",current degree:"+currentDegree);
            if ((Mathf.Abs(currentDegree - degree) == 90) || (Mathf.Abs(currentDegree - degree - 360) == 90) || (Mathf.Abs(currentDegree - degree + 360) == 90))//|| (degree > 165 && degree < 195) || (degree > 255 && degree < 285) || (degree > 345 || degree < 15)
            {
                if ((degree == 90))
                {
                    //Debug.Log("points found in top");
                    pointsAtTop.Add(point);
                }
                else if ((degree == 180))
                {
                    //Debug.Log("points found in left");
                    pointsAtLeft.Add(point);
                }
                else if ((degree == 270))
                {
                    //Debug.Log("points found in down");
                    pointsAtBottom.Add(point);
                }
                else if ((degree == 360))
                {
                    //Debug.Log("points found in right");
                    pointsAtRight.Add(point);
                }
            }
            foreach (Vector3 p in pointsAtTop)
            {
                float tempDist = Vector2.Distance(currentPoint, p);
                if (tempDist < distance)
                {
                    topPoint = p;
                    distance = tempDist;
                }
            }
            distance = float.MaxValue;
            foreach (Vector3 p in pointsAtLeft)
            {
                float tempDist = Vector2.Distance(currentPoint, p);
                if (tempDist < distance)
                {
                    leftPoint = p;
                    distance = tempDist;
                }
            }
            distance = float.MaxValue;
            foreach (Vector3 p in pointsAtBottom)
            {
                float tempDist = Vector2.Distance(currentPoint, p);
                if (tempDist < distance)
                {
                    bottomPoint = p;
                    distance = tempDist;
                }
            }
            distance = float.MaxValue;
            foreach (Vector3 p in pointsAtRight)
            {
                float tempDist = Vector2.Distance(currentPoint, p);
                if (tempDist < distance)
                {
                    rightPoint = p;
                    distance = tempDist;
                }
            }
        }
        if (pointsAtTop.Count != 0)
        {
            //Debug.Log("addedd");
            List<Vector2> tempOrderedList = new List<Vector2>(orderedList);
            tempOrderedList.Add(topPoint);
            polygonList = FindPolygons(tempPointList, topPoint, 90, polygonList, firstPoint, tempOrderedList, false);
        }
        if (pointsAtLeft.Count != 0)
        {
            //Debug.Log("addedd");
            List<Vector2> tempOrderedList = new List<Vector2>(orderedList);
            tempOrderedList.Add(leftPoint);
            polygonList = FindPolygons(tempPointList, leftPoint, 180, polygonList, firstPoint, tempOrderedList, false);
        }
        if (pointsAtBottom.Count != 0)
        {
            //Debug.Log("addedd");
            List<Vector2> tempOrderedList = new List<Vector2>(orderedList);
            tempOrderedList.Add(bottomPoint);
            polygonList = FindPolygons(tempPointList, bottomPoint, 270, polygonList, firstPoint, tempOrderedList, false);
        }
        if (pointsAtRight.Count != 0)
        {
            //Debug.Log("addedd");
            List<Vector2> tempOrderedList = new List<Vector2>(orderedList);
            tempOrderedList.Add(rightPoint);
            polygonList = FindPolygons(tempPointList, rightPoint, 360, polygonList, firstPoint, tempOrderedList, false);
        }

        return polygonList;
    }
}
