using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPointsManager : MonoBehaviour
{
    public Points points;
    public thePoint curPoint;
    public thePoint lastPoint;
    public thePoint nextPoint;
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
        //print(curPoint.location);
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
        lastPoint = points.myPoints[idx];
        curPoint = points.getNextPoint(lastPoint, 0);
        if (curPoint == null)
        {
            init(idx);
            return;
        }

        this.transform.position = lastPoint.location;
    }

    public void getNext(int index)
    {
        //print("index: "+index+"  pointnya: "+points.getPointsCount());
        index = index < points.getNextPointsCount(curPoint) ? index : 0;
        lastPoint = curPoint;

        //curPoint = points.getNextPoint(lastPoint, index);
        try
        {
            string name = lastPoint.next[index].obj.name;
            curPoint = points.getPointByName(name);
        }
        catch (Exception e)
        {
            //Destroy(this.gameObject);
            int idx = UnityEngine.Random.Range(0, points.myPoints.Count - 1);
            init(idx);
        }

        //if (curPoint == null) 
            
    }

    public void setCurPoint(int index)
    {
        index = index < points.getPointsCount() ? index : 0;
        curPoint = points.myPoints[index];
    }

    public Vector2 getCurLocation(int laneku)
    {
        //print(lastPoint.location + "  " + curPoint.location);
        Vector2 temp = getLocationDirection();
        Vector2 vecL= Vector2.Perpendicular(temp);

        //if (lastPoint.obj.name == curPoint.obj.name) Destroy(this.gameObject);

        return curPoint.location + vecL*(-laneku*laneWidth + laneStartPos);
    }

    public Vector2 getRawCurLocation()
    {
        return curPoint.location;
    }

    public Vector2 getRawNextLocationDir()
    {
        Vector2 mDir = new Vector2(nextPoint.location.x - curPoint.location.x, nextPoint.location.y - curPoint.location.y).normalized;
        return mDir;
    }

    public Vector2 getLocationDirection()
    {
        Vector2 dir = new Vector2(curPoint.location.x - lastPoint.location.x, curPoint.location.y - lastPoint.location.y).normalized;
        return dir;
    }

    public Vector2 getNextLocationDir(int laneku)
    {
        Vector2 mDir = points.getNextPointLocation(curPoint, laneNum);
        print(mDir);
        if (mDir == Vector2.zero) return mDir;

        mDir = new Vector2(mDir.x - curPoint.location.x, mDir.y - curPoint.location.y).normalized;
        return mDir;
    }

    public int getMaxLane()
    {
        return laneNum;
    }

    public float getLaneWidth()
    {
        return laneWidth;
    }

}
