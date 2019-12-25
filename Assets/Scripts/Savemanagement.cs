using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savemanagement : MonoBehaviour
{
    private UIcontroller uicontroller;

    public int money;
    public float fuel;
    public float wantedlvl;
    public int repair;

    void Start()
    {
        money = PlayerPrefs.GetInt("Curmoney", 0);
        fuel = PlayerPrefs.GetFloat("Curfuel", 1);
        wantedlvl = PlayerPrefs.GetFloat("Wantedlvl", 0);
        repair = PlayerPrefs.GetInt("Repair", 100);

        uicontroller = FindObjectOfType(typeof(UIcontroller)) as UIcontroller;

        uicontroller.money();
        uicontroller.loadfuel();
        uicontroller.loadwanted();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }


}
