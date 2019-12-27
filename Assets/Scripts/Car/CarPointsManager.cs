using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPointsManager : MonoBehaviour
{
    public Points points;
    public thePoint curPoint;
    public thePoint lastPoint;
    private float laneWidth = 1.5f;     //width of a single lane
    private int laneNum = 2;           //number of lanes
    private float laneStartPos;

    public void Awake()
    {
        print(points.getPointsCount());
        curPoint = points.myPoints[0];
        //lastPoint = curPoint.next[0];
        lastPoint = points.getNextPoint(curPoint, 0);

        this.transform.position = curPoint.location;

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

    public void getNext(int index)
    {
        //print("index: "+index+"  pointnya: "+points.getPointsCount());
        index = index < points.getNextPointsCount(curPoint) ? index : 0;
        lastPoint = curPoint;

        //curPoint = points.getNextPoint(lastPoint, index);
        string name = lastPoint.next[0].obj.name;
        curPoint =  points.getPointByName(name);

        print(lastPoint + "  " + curPoint);
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
        return curPoint.location + vecL*(-laneku*laneWidth + laneStartPos);
    }

    public Vector2 getLocationDirection()
    {
        Vector2 dir = new Vector2(curPoint.location.x - lastPoint.location.x, curPoint.location.y - lastPoint.location.y).normalized;
        return dir;
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
