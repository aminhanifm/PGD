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
    [SerializeField]public List<Tuple<Location, Location>> objective;
    public List<Location> curObjective;

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
            getNextDestination();
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
            save.money += 50000;
            SoundsManager.PlaySound("Mission Done");
            success = true;
            save.indexmission += 1;
            save.mission += 1;
            PlayerPrefs.SetInt("IndexMission", save.indexmission);
            PlayerPrefs.SetInt("Mission", save.mission);
            PlayerPrefs.SetInt("Curmoney", save.money);
            currentmission(save.indexmission);
            //getNextDestination();
        }
    }

    public void currentmission(int index)
    {
        if (index == 1)
        {
            AddCustomObjective(12, 18);
            getNextDestination();
            print("Misi 1");
        }
        else if (index == 2)
        {
            AddCustomObjective(13, 6);
            //getNextDestination();
            print("Misi 2");
        }
    }
}
