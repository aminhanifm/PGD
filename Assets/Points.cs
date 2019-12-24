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

public class Points : MonoBehaviour
{
    public List<thePoint> myPoints = new List<thePoint>();

    public void Awake()
    {
        thePoint one = createPoint(new Vector2(0, 0));
        thePoint two = createPoint(new Vector2(20, 10));
        thePoint three = createPoint(new Vector2(0, 20));
        thePoint four = createPoint(new Vector2(-20, 10));

        thePoint five = createPoint(new Vector2(40, 0));
        thePoint six = createPoint(new Vector2(20, -10));

        addNextPoint(one, new thePoint[] { two });
        addNextPoint(two, new thePoint[] { three, five });
        addNextPoint(three, new thePoint[] { four });
        addNextPoint(four, new thePoint[] { one });
        addNextPoint(five, new thePoint[] { six });
        addNextPoint(six, new thePoint[] { one });
    }

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

    public int getNextPointsCount(thePoint point)
    {
        return point.next.Count;
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        //HandleUtility.Repaint();

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.color = Color.green;

        foreach (var pt in myPoints)
        {
            Gizmos.DrawSphere(pt.location, 1);
            foreach (var pt_next in pt.next)
            {
                Gizmos.DrawLine(pt.location, pt_next.location);
            }
        }
    }
#endif
}