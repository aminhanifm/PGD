using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class thePoint
{
    public List<thePoint> next;
    public Vector2 location;

    public thePoint(Vector2 location)
    {
        this.location = location;
        this.next = new List<thePoint>();
    }
}

public class Points
{
    public List<thePoint> myPoints = new List<thePoint>();
    
    public thePoint createPoint(Vector2 loc)
    {
        thePoint point = new thePoint(loc);
        myPoints.Add(point);
        return point;
    }

    public void addNextPoint(thePoint point, thePoint[] lsPoint)
    {
        foreach(var pt in lsPoint)
        {
            point.next.Add(pt);
        }
    }

    public thePoint getNextPoint(thePoint point, int idx=0)
    {
        thePoint nextPoint;
        try
        {
            nextPoint = point.next[idx];
        }catch(Exception e)
        {
            return null;
        }

        return nextPoint;
    }

    public int getPointsCount()
    {
        return myPoints.Count;
    }
}