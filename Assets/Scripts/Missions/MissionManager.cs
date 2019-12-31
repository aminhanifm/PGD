using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LocationManager))]
public class MissionManager : MonoBehaviour
{
    Savemanagement savemanagement;
    DialogueManager dm;
    KoganeUnityLib.dialogueTemplate dt;
    UIcontroller ui;
    isocar_controller iso;

    public GameObject locationTrigger;
    public LocationManager LM;
    [SerializeField]public List<Tuple<Location, Location>> objective;
    public List<Location> curObjective;
    

    bool success;
    Location curDestination;

    void Start()
    {
        savemanagement = FindObjectOfType(typeof(Savemanagement)) as Savemanagement;
        dm = FindObjectOfType(typeof(DialogueManager)) as DialogueManager;
        dt = FindObjectOfType(typeof(KoganeUnityLib.dialogueTemplate)) as KoganeUnityLib.dialogueTemplate;
        ui = FindObjectOfType(typeof(UIcontroller)) as UIcontroller;
        iso = FindObjectOfType(typeof(isocar_controller)) as isocar_controller;
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
            currentmission(savemanagement.indexmission);
        }
    }

    public void init()
    {
        success = false;
        generateObjective(3);
    }

    public void getNextDestination()
    {
        //if (curObjective.Count < 1)
        //{
        //    setCurObjective();
        //}

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
            print(mCur.Item1.name + " " + mCur.Item2.name);
            curObjective.Add(mCur.Item1);
            curObjective.Add(mCur.Item2);
            objective.RemoveAt(0);
            print("berhasil masukno");
        }
        catch(Exception e)
        {
            //trigger objective selesai
            print("LAH");
            savemanagement.money += 15000;
            SoundsManager.PlaySound("Mission Done");
            success = true;
            savemanagement.indexmission += 1;
            savemanagement.mission += 1;
            PlayerPrefs.SetInt("IndexMission", savemanagement.indexmission);
            PlayerPrefs.SetInt("Mission", savemanagement.mission);
            PlayerPrefs.SetInt("Curmoney", savemanagement.money);
            currentmission(savemanagement.indexmission);
            dm.initScenario(savemanagement.mission);
            dm.continueLine();

            //getNextDestination();
        }
    }


    public void currentmission(int index)
    {
        if (index == 1)
        {
            AddCustomObjective(12, 18);
            setCurObjective();
            getNextDestination();
            print("Misi 1");
        }
        else if (index == 2)
        {
            AddCustomObjective(13, 6);
            getNextDestination();
            print("Misi 2");
        }
        if (index == 3)
        {
            AddCustomObjective(14, 2);
            //getNextDestination();
            print("Misi 3");
        }
        if (index == 4)
        {
            AddCustomObjective(3, 8);
            //getNextDestination();
            print("Misi 4");
        }
        if (index == 5)
        {
            AddCustomObjective(19, 1);
            //getNextDestination();
            print("Misi 5");
        }
        if (index == 6)
        {
            AddCustomObjective(12, 6);
            //getNextDestination();
            print("Misi 6");
        }
        if (index == 7)
        {
            AddCustomObjective(11, 3);
            //getNextDestination();
            print("Misi 6");
    }
        if (index == 8)
        {
            AddCustomObjective(9, 16);
            //getNextDestination();
            print("Misi 6");
        }
    }
}
