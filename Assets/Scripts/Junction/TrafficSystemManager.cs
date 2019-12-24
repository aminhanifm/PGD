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
        //print(all[trafficAt].isStop);
        if (all[trafficAt].isStop)
        {
            print("ASS");
            trafficAt = (trafficAt + 1) % trafficsCount;
            changeTraffic();
        }
    }

    void changeTraffic()
    {
        all[trafficAt].isStop = false;
        //all[trafficAt].isStart = true;
        int dur = all[trafficAt].getToYellowDuration() + 3;
        all[trafficAt].setAnimatorAt(1);

        for (int i = 0; i < trafficsCount; i++)
        {
            if (i != trafficAt)
            {
                all[i].isStop = true;
                //all[trafficAt].isStart = true;
                all[i].setToGreenDuration(dur);
                all[i].setAnimatorAt(0);

            }
        }
    }
}