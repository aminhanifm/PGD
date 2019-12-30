using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savemanagement : MonoBehaviour
{
    private UIcontroller uicontroller;
    private MissionManager missionmanager;
    private bool isload = true;

    public int mission;
    public int indexmission;

    public int money;
    public float fuel;
    public float wantedlvl;
    public int repair;

    void Start()
    {
        PlayerPrefs.DeleteAll();
        mission = PlayerPrefs.GetInt("Mission", 2);
        indexmission = PlayerPrefs.GetInt("IndexMission", 1);
        money = PlayerPrefs.GetInt("Curmoney", 0);
        fuel = PlayerPrefs.GetFloat("Curfuel", 1);
        wantedlvl = PlayerPrefs.GetFloat("Wantedlvl", 0);
        repair = PlayerPrefs.GetInt("Repair", 100);
       
        uicontroller = FindObjectOfType(typeof(UIcontroller)) as UIcontroller;
        missionmanager = FindObjectOfType(typeof(MissionManager)) as MissionManager;


        missionmanager.currentmission(indexmission);
        //missionmanager.getNextDestination();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        print(indexmission);
        //load
        if (uicontroller.MMCanvas.alpha == 0 && uicontroller.fadingmm == true && isload == true)
        {
            uicontroller.loadfuel();
            uicontroller.loadwanted();
            uicontroller.loadrepair();
            isload = false;
        }

        if(repair < 0)
        {
            repair = 0;
        }

    }

    public void Newgame()
    {
        indexmission = PlayerPrefs.GetInt("IndexMission", 1);
        mission = PlayerPrefs.GetInt("Mission", 2);
        money = PlayerPrefs.GetInt("Curmoney", 0);
        fuel = PlayerPrefs.GetFloat("Curfuel", 1);
        wantedlvl = PlayerPrefs.GetFloat("Wantedlvl", 0);
        repair = PlayerPrefs.GetInt("Repair", 100);
    }

}
