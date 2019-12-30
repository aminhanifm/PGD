using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSystemManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform traffics;
    private int trafficsCount;
    private int trafficAt;
    private List<TrafficLight> all;

    void Start()
    {
        all = new List<TrafficLight>();
        trafficAt = 0;
        trafficsCount = traffics.childCount;


        //for(int i=0; i<trafficsCount; i++)
        //{
        //    all.Add(traffics.GetChild(i).GetComponent<TrafficLight>());
        //}

        foreach (Transform child in traffics.transform)
        {
            //print(child.name);
            all.Add(child.transform.GetComponent<TrafficLight>());
        }

        changeTraffic();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (all[trafficAt].isStop)
        {
            int newidx =(trafficAt + 1) % trafficsCount;
            if(newidx != trafficAt)
            {
                trafficAt = newidx;
                for (int i = 0; i < trafficsCount; i++)
                {
                    all[i].stopAnimator();
                }
                changeTraffic();
            }
        }

    }

    void changeTraffic()
    {
        int dur = all[trafficAt].getToYellowDuration() + 4;

        for (int i = 0; i < trafficsCount; i++)
        {
            if (i != trafficAt)
            {
                all[i].isStop = true;
                all[i].setToGreenDuration(dur);
                all[i].setAnimatorAt(0);

            }
            else
            {
                all[trafficAt].isStop = false;
                all[trafficAt].setAnimatorAt(1);
            }
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        //HandleUtility.Repaint();

        //Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        Gizmos.color = Color.magenta;

        Transform temp = null;

        foreach (Transform child in traffics.transform)
        {
            if(temp!=null)
                Gizmos.DrawLine(child.transform.position, temp.transform.position);

            temp = child.transform;
        }
    }
#endif
}