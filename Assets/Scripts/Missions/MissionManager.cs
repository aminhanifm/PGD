using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LocationManager))]
public class MissionManager : MonoBehaviour
{
    Savemanagement save;
    DialogueManager dialogmanager;
    KoganeUnityLib.dialogueTemplate dt;
    UIcontroller ui;

    public GameObject locationTrigger;
    public LocationManager LM;
    public List<Tuple<Location, Location>> objective;
    private List<Location> curObjective;

    bool success;
    Location curDestination;

    void Start()
    {
        save = FindObjectOfType(typeof(Savemanagement)) as Savemanagement;
        dialogmanager = FindObjectOfType(typeof(DialogueManager)) as DialogueManager;
        dt = FindObjectOfType(typeof(KoganeUnityLib.dialogueTemplate)) as KoganeUnityLib.dialogueTemplate;
        ui = FindObjectOfType(typeof(UIcontroller)) as UIcontroller;
        LM = gameObject.GetComponent<LocationManager>();
        objective= new List<Tuple<Location, Location>>();
        curObjective = new List<Location>();

        //demo
        //init();
    }

    private void Update()
    {
        //demo
        if (Input.GetKeyDown(KeyCode.J))
        {
            currentmission(save.indexmission);
        }
    }

    public void init()
    {
        success = false;
        generateObjective(3);
    }

    public void getNextDestination()
    {
        if (curObjective.Count < 1)
        {
            setCurObjective();
        }

        curDestination = curObjective[0];
        locationTrigger.transform.position = curDestination.position;
        curObjective.RemoveAt(0);
    }

    //untuk membuat misi kustom
    public void AddCustomObjective(int mFrom, int mTo)
    {
        LM.MakeLocationPair(mFrom, mTo, out Location from, out Location to);
        objective.Add(Tuple.Create(from, to));
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
            print("LAH");
            save.money += 15000;
            SoundsManager.PlaySound("Mission Done");
            success = true;
            save.indexmission += 1;
            save.mission += 1;
            PlayerPrefs.SetInt("IndexMission", save.indexmission);
            PlayerPrefs.SetInt("Mission", save.mission);
            PlayerPrefs.SetInt("Curmoney", save.money);
            currentmission(save.indexmission);
        }
    }

    public void currentmission(int index)
    {
        if (index == 1)
        {
            AddCustomObjective(12, 13);
            //getNextDestination();
            print("Misi 1");
        }
        if (index == 2)
        {
            AddCustomObjective(6, 15);
            //getNextDestination();
            print("Misi 2");
        }
        if (index == 3)
        {
            AddCustomObjective(3, 4);
            //getNextDestination();
            print("Misi 3");
        }
        if (index == 4)
        {
            AddCustomObjective(9, 20);
            //getNextDestination();
            print("Misi 4");
        }
        if (index == 5)
        {
            AddCustomObjective(2, 12);
            //getNextDestination();
            print("Misi 5");
        }
        if (index == 6)
        {
            AddCustomObjective(5, 21);
            //getNextDestination();
            print("Misi 6");
        }
    }
}
