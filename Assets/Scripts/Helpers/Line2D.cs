using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line2D 
{
    public double X1 = 0;
    public double X2 = 0;
    public double Y1 = 0;
    public double Y2 = 0;

    public Vector2 P1;
    public Vector2 P2;

    public Line2D(Vector2 p1, Vector2 p2)
    {
        P1 = p1;
        P2 = p2;
        X1 = p1.x;
        X2 = p2.x;
        Y1 = p1.y;
        Y2 = p2.y;
    }

    public double getX1()
    {
        return X1;
    }
    public double getX2()
    {
        return X2;
    }
    public double getY1()
    {
        return Y1;
    }
    public double getY2()
    {
        return Y2;
    }

    public Vector2 getP1()
    {
        return P1;
    }

    public Vector2 getP2()
    {
        return P2;
    }

    public bool intersectsLine(Line2D comparedLine)
    {
        if (X2 == comparedLine.X1 && Y2 == comparedLine.Y1)
        {
            return false;
        }

        if (X1 == comparedLine.X2 && Y1 == comparedLine.Y2)
        {
            return false;
        }
        double firstLineSlopeX, firstLineSlopeY, secondLineSlopeX, secondLineSlopeY;

        firstLineSlopeX = X2 - X1;
        firstLineSlopeY = Y2 - Y1;

        secondLineSlopeX = comparedLine.getX2() - comparedLine.getX1();
        secondLineSlopeY = comparedLine.getY2() - comparedLine.getY1();

        double s, t;
        s = (-firstLineSlopeY * (X1 - comparedLine.getX1()) + firstLineSlopeX * (getY1() - comparedLine.getY1())) / (-secondLineSlopeX * firstLineSlopeY + firstLineSlopeX * secondLineSlopeY);
        t = (secondLineSlopeX * (getY1() - comparedLine.getY1()) - secondLineSlopeY * (getX1() - comparedLine.getX1())) / (-secondLineSlopeX * firstLineSlopeY + firstLineSlopeX * secondLineSlopeY);
        //Debug.Log("data:"+s+","+t);
        if (s >= 0 && s < 1 && t >= 0 && t < 1)
        {
            return true;
        }

        return false; // No collision
    }

    public bool doBoundingBoxesIntersect(Line2D a, Line2D b)
    {
        return a.P1.x <= b.P2.x
            && a.P2.x >= b.P1.x
            && a.P1.y <= b.P2.y
            && a.P2.y >= b.P1.y;
    }


    public override int GetHashCode()
    {
        return (X1 * 1000 + X2 * 1000 + Y1 * 1000 + Y2 * 1000).GetHashCode();
    }

    public override bool Equals(object obj)
    {
        return (obj.GetHashCode() == this.GetHashCode());
    }

    class endpointEntry
    {
        public double XValue;
        public bool isHi;
        public Line2D line;
        public double hi;
        public double lo;
        public endpointEntry fLink;
        public endpointEntry bLink;
    }

    class endpointSorter : IComparer<endpointEntry>
    {
        public int Compare(endpointEntry c1, endpointEntry c2)
        {
            // sort values on XValue, descending
            if (c1.XValue > c2.XValue) { return -1; }
            else if (c1.XValue < c2.XValue) { return 1; }
            else // must be equal, make sure hi's sort before lo's
                if (c1.isHi && !c2.isHi) { return -1; }
            else if (!c1.isHi && c2.isHi) { return 1; }
            else { return 0; }
        }
    }

    public bool CheckForCrossing(List<Line2D> lines)
    {
        List<endpointEntry> pts = new List<endpointEntry>(2 * lines.Count);

        // Make endpoint objects from the lines so that we can sort all of the
        // lines endpoints.
        foreach (Line2D lin in lines)
        {
            // make the endpoint objects for this line
            endpointEntry hi, lo;
            if (lin.P1.x < lin.P2.x)
            {
                hi = new endpointEntry() { XValue = lin.P2.x, isHi = true, line = lin, hi = lin.P2.x, lo = lin.P1.x };
                lo = new endpointEntry() { XValue = lin.P1.x, isHi = false, line = lin, hi = lin.P1.x, lo = lin.P2.x };
            }
            else
            {
                hi = new endpointEntry() { XValue = lin.P1.x, isHi = true, line = lin, hi = lin.P1.x, lo = lin.P2.x };
                lo = new endpointEntry() { XValue = lin.P2.x, isHi = false, line = lin, hi = lin.P2.x, lo = lin.P1.x };
            }
            // add them to the sort-list
            pts.Add(hi);
            pts.Add(lo);
        }

        // sort the list
        pts.Sort(new endpointSorter());

        // sort the endpoint forward and backward links
        endpointEntry prev = null;
        foreach (endpointEntry pt in pts)
        {
            if (prev != null)
            {
                pt.bLink = prev;
                prev.fLink = pt;
            }
            prev = pt;
        }

        // NOW, we are ready to look for intersecting lines
        foreach (endpointEntry pt in pts)
        {
            // for every Hi endpoint ...
            if (pt.isHi)
            {
                // check every other line whose X-range is either wholly 
                // contained within our own, or that overlaps the high 
                // part of ours.  The other two cases of overlap (overlaps 
                // our low end, or wholly contains us) is covered by hi 
                // points above that scan down to check us.

                // scan down for each lo-endpoint below us checking each's 
                // line for intersection until we pass our own lo-X value
                for (endpointEntry pLo = pt.fLink; (pLo != null) && (pLo.XValue >= pt.lo); pLo = pLo.fLink)
                {
                    // is this a lo-endpoint?
                    if (!pLo.isHi)
                    {
                        // check its line for intersection
                        if (pt.line.intersectsLine(pLo.line))
                            return true;
                    }
                }
            }
        }

        return false;
    }
}
