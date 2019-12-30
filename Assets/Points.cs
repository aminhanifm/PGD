using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class LocPoint {
    public GameObject obj;
    public List<string> nextKey;

    public LocPoint(GameObject obj)
    {
        this.obj = obj;
        this.nextKey = new List<string>();
    }
}

[Serializable] 
public class LocPointsDictionary : SerializableDictionary<string, LocPoint> { }

public class Points : MonoBehaviour
{
    [SerializeField]
    private LocPointsDictionary lockeystore = LocPointsDictionary.New<LocPointsDictionary>();
    public Dictionary<string, LocPoint> lockeys
    {
        get { return lockeystore.dictionary; }
    }


    //?Eventual script
    //public void GenerateLocPoints()
    //{
    //    foreach (var pt in myPoints)
    //    {
    //        LocPoint baru = new LocPoint(pt.obj);

    //        for (var i=0; i<pt.next.Count; i++)
    //        {
    //            baru.nextKey.Add(pt.next[i].obj.name);
    //        }

    //        lockeys.Add(pt.obj.name, baru);
    //    }
    //}

    //public thePoint createPoint(Vector2 loc)
    //{
    //    thePoint point = new thePoint(loc);
    //    myPoints.Add(point);
    //    return point;
    //}

    //public thePoint createPoint(GameObject obj)
    //{
    //    thePoint point = new thePoint(obj);
    //    myPoints.Add(point);
    //    return point;
    //}

    //public void addNextPoint(thePoint point, thePoint[] lsPoint)
    //{
    //    foreach(var pt in lsPoint)
    //    {
    //        point.next.Add(pt);
    //    }
    //}

    public LocPoint getNextPoint(LocPoint point, int idx = 0)
    {
        LocPoint nextPoint;
        try
        {
            nextPoint = lockeys[point.nextKey[idx]];
        }
        catch (Exception e)
        {
            return null;
        }

        return nextPoint;
    }

    //public Vector2 getNextPointLocation(thePoint point, int idx = 0)
    //{
    //    try
    //    {
    //        //print(point.next[idx].obj.name);
    //        return point.next[idx].location;
    //    }
    //    catch (Exception e)
    //    {
    //        return Vector2.zero;
    //    }
    //}

    public int getPointsCount()
    {
        return lockeys.Count;
    }

    public int getNextPointsCount(LocPoint point)
    {
        return point.nextKey.Count;
    }

    public LocPoint getPointByKey(string key)
    {
        if(lockeys.TryGetValue(key, out LocPoint result))
        {
            return result;
        }
        return null;
    }

}