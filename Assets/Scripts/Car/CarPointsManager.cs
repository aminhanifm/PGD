using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPointsManager : MonoBehaviour
{
    public Points points = new Points();
    public thePoint curPoint;
    public thePoint lastPoint;
    private float laneWidth = 1.2f;     //width of a single lane
    private int laneNum = 2;           //number of lanes
    private float laneStartPos;

    public void Awake()
    {
        thePoint one = points.createPoint(new Vector2(0, 0));
        thePoint two = points.createPoint(new Vector2(20, 10));
        thePoint three = points.createPoint(new Vector2(0, 20));
        thePoint four = points.createPoint(new Vector2(-20, 10));

        thePoint five = points.createPoint(new Vector2(40, 0));
        thePoint six = points.createPoint(new Vector2(20, -10));

        points.addNextPoint(one, new thePoint[] {two});
        points.addNextPoint(two, new thePoint[] {three, five});
        points.addNextPoint(three, new thePoint[] {four});
        points.addNextPoint(four, new thePoint[] { one });
        points.addNextPoint(five, new thePoint[] {six});
        points.addNextPoint(six, new thePoint[] {one});

        curPoint = points.myPoints[0];
        lastPoint = curPoint;

        laneStartPos = (laneNum * laneWidth) / 2;       //define length from point's origin to the most left
        //print(points.myPoints[0].location);
    }

    public void getNext(int index)
    {
        index = index < points.getPointsCount() ? index : 0;
        lastPoint = curPoint;
        curPoint = points.getNextPoint(curPoint, index);
    }

    public void setCurPoint(int index)
    {
        index = index < points.getPointsCount() ? index : 0;
        curPoint = points.myPoints[index];
    }

    public Vector2 getCurLocation(int laneku)
    {
        Vector2 temp = new Vector2(curPoint.location.x - lastPoint.location.x, curPoint.location.y - lastPoint.location.y).normalized;
        Vector2 vecL= Vector2.Perpendicular(temp);
        return curPoint.location + vecL*(-laneku*laneWidth + laneStartPos);
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        //HandleUtility.Repaint();

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.color = Color.green;

        foreach (var pt in points.myPoints)
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
