using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LocationManager))]
public class MissionManager : MonoBehaviour
{
    public GameObject locationTrigger;
    [HideInInspector] public LocationManager LM;
    [HideInInspector] public List<Tuple<Location, Location>> objective;
    private List<Location> curObjective;
    bool success;
    Location curDestination;

    void Start()
    {
        LM = gameObject.GetComponent<LocationManager>();
        objective= new List<Tuple<Location, Location>>();
        curObjective = new List<Location>();

        init();
    }

    public void init()
    {
        success = false;
        generateObjective(3);

    }

    public void getNextDestination()
    {
        curDestination = curObjective[0];
        locationTrigger.transform.position = curDestination.position;
        curObjective.RemoveAt(0);

        if (curObjective.Count < 1)
        {
            setCurObjective();
        }
    }

    public void generateObjective(int totalObjective)
    {
        for(int i=0; i<totalObjective; i++)
        {
            LM.GenerateLocationPair(out Location from, out Location to);
            objective.Add(Tuple.Create(from, to));
        }
    }

    public void setCurObjective()
    {
        try
        {
            Tuple<Location, Location> mCur = objective[0];
            curObjective.Add(mCur.Item1);
            curObjective.Add(mCur.Item2);
            objective.RemoveAt(0);
        }
        catch(Exception e)
        {
            //trigger objective selesai
            success = true;
        }
    }
}
