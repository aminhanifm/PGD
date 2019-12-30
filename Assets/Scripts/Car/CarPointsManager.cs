using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPointsManager : MonoBehaviour
{
    public Points points;
    public LocPoint curPoint;
    public LocPoint lastPoint;
    private float laneWidth = 1.5f;     //width of a single lane
    private int laneNum = 2;           //number of lanes
    private float laneStartPos;

    public void Awake()
    {
        //curPoint = points.myPoints[0];
        ////lastPoint = curPoint.next[0];
        //lastPoint = points.getNextPoint(curPoint, 0);
        points = GameObject.Find("Car Way Points").GetComponent<Points>();
        laneStartPos = (-laneWidth + laneNum * laneWidth) / 2;       //define length from point's origin to the most left

        //sementara
        int idx = UnityEngine.Random.Range(0, points.getPointsCount() - 1);
        init(idx);
    }
    //public void start()
    //{

    //    points = PointGameObject.GetComponent<Points>();
    //    curPoint = points.myPoints[0];
    //    lastPoint = curPoint;
    //    //print(curPoint.location);
    //    laneStartPos = (- laneWidth + laneNum * laneWidth) / 2;       //define length from point's origin to the most left
    //}

    public void init(int idx)
    {
        lastPoint = points.lockeys[idx.ToString()];
        curPoint = points.getNextPoint(lastPoint, 0);
        if (curPoint == null)
        {
            init(idx);
            return;
        }

        this.transform.position = lastPoint.obj.transform.position;
    }

    public void getNext(int index)
    {
        //print("index: "+index+"  pointnya: "+points.getPointsCount());
        index = index < points.getNextPointsCount(curPoint) ? index : 0;
        lastPoint = curPoint;

        //curPoint = points.getNextPoint(lastPoint, index);
        try
        {
            string key = lastPoint.nextKey[index];
            curPoint = points.getPointByKey(key);
        }
        catch (Exception e)
        {
            //Destroy(this.gameObject);
            print("rusak");
            int idx = UnityEngine.Random.Range(0, points.lockeys.Count - 1);
            init(idx);
        }
            
    }

    //public void setCurPoint(int index)
    //{
    //    index = index < points.getPointsCount() ? index : 0;
    //    curPoint = points.myPoints[index];
    //}

    public Vector2 getCurLocation(int laneku)
    {
        //print(lastPoint.location + "  " + curPoint.location);
        Vector2 temp = getLocationDirection();
        Vector2 vecL= Vector2.Perpendicular(temp);

        //if (lastPoint.obj.name == curPoint.obj.name) Destroy(this.gameObject);
        Vector2 pos = curPoint.obj.transform.position;
        return pos + vecL*(-laneku*laneWidth + laneStartPos);
    }

    public Vector2 getRawCurLocation()
    {
        return curPoint.obj.transform.position;
    }

    //public Vector2 getRawNextLocationDir()
    //{
    //    Vector2 mDir = new Vector2(nextPoint.location.x - curPoint.location.x, nextPoint.location.y - curPoint.location.y).normalized;
    //    return mDir;
    //}

    public Vector2 getLocationDirection()
    {
        Vector2 cur = curPoint.obj.transform.position;
        Vector2 last = lastPoint.obj.transform.position;
        Vector2 dir = new Vector2(cur.x - last.x, cur.y - last.y).normalized;
        return dir;
    }

    //public Vector2 getNextLocationDir(int laneku)
    //{
    //    Vector2 mDir = points.getNextPointLocation(curPoint, laneNum);
    //    print(mDir);
    //    if (mDir == Vector2.zero) return mDir;

    //    mDir = new Vector2(mDir.x - curPoint.location.x, mDir.y - curPoint.location.y).normalized;
    //    return mDir;
    //}

    public int getMaxLane()
    {
        return laneNum;
    }

    public float getLaneWidth()
    {
        return laneWidth;
    }

}
