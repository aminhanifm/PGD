using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[System.Serializable]
public class thePoint
{
    public GameObject obj;
    public List<thePoint> next;
    public Vector2 location;

    public thePoint(GameObject obj)
    {
        this.obj = obj;
        this.location = obj.transform.position;
        this.next = new List<thePoint>();
    }

    public thePoint(Vector2 location)
    {
        this.location = location;
        this.next = new List<thePoint>();
    }
}

public class Points : MonoBehaviour
{
    [SerializeField] public List<thePoint> myPoints = new List<thePoint>();

    public void Awake()
    {
        //thePoint one = createPoint(new Vector2(0, 0));
        //thePoint two = createPoint(new Vector2(20, 10));
        //thePoint three = createPoint(new Vector2(0, 20));
        //thePoint four = createPoint(new Vector2(-20, 10));

        //thePoint five = createPoint(new Vector2(40, 0));
        //thePoint six = createPoint(new Vector2(20, -10));

        //addNextPoint(one, new thePoint[] { two });
        //addNextPoint(two, new thePoint[] { three, five });
        //addNextPoint(three, new thePoint[] { four });
        //addNextPoint(four, new thePoint[] { one });
        //addNextPoint(five, new thePoint[] { six });
        //addNextPoint(six, new thePoint[] { one });
    }

    public thePoint createPoint(Vector2 loc)
    {
        thePoint point = new thePoint(loc);
        myPoints.Add(point);
        return point;
    }

    public thePoint createPoint(GameObject obj)
    {
        thePoint point = new thePoint(obj);
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

    public thePoint getPointByName(string name)
    {
        foreach (var item in myPoints)
        {
            if (item.obj.name == name)
            {
                return item;
            }
        }

        return null;
    }

}