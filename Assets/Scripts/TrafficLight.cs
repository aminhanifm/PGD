using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{

    [HideInInspector] public int traffictoyellow;
    [HideInInspector] public int traffictogreen;
    public Animator trafficanimator;
    public int type; //set type from 1 to 4 which has time differences
    public string trafficname; //only for 2 or more traffics
    public bool isCounting; //must set to yes
    public bool ismanytraffic; //set yes if need to compare with other traffic light
    private bool triggeronce;
    public int firstcondition; // set 1 to make it start animating

    void Start()
    {
        trafficanimator = gameObject.GetComponent<Animator>();
        if(type == 1)
        {
            type1();
        }
        if (type == 2)
        {
            type2();
        }
        if (type == 3)
        {
            type3();
        }
        if (type == 4)
        {
            type4();
        }
        trafficanimator.SetInteger("Color",firstcondition);

    }

    void Update()
    {
        //testing purpose only
        if (Input.GetButtonDown("Jump"))
        {
            trafficanimator.SetInteger("Color", 1);
            StartCoroutine(startlight());
            //Debug.Log("Succed");
        }
    }

    private void FixedUpdate()
    {
        if (isCounting && !triggeronce)
        {
            StartCoroutine(startlight());
            triggeronce = true;
            //Debug.Log("Succed");
        }
    }

    public void type1()
    {
        traffictogreen = 7;
        traffictoyellow = 5;
    }

    public void type2()
    {
        traffictogreen = 9;
        traffictoyellow = 7;
    }

    public void type3()
    {
        traffictogreen = 11;
        traffictoyellow = 9;
    }

    public void type4()
    {
        traffictogreen = 13;
        traffictoyellow = 11;
    }

    IEnumerator startlight()
    {
        if(!ismanytraffic)
        {
            //to green
            if (trafficanimator.GetInteger("Color") == 0)
            {
                yield return new WaitForSeconds(traffictogreen);
                trafficanimator.SetInteger("Color", 1);
            }
            //to yellow
            if (trafficanimator.GetInteger("Color") == 1)
            {
                yield return new WaitForSeconds(traffictoyellow);
                trafficanimator.SetInteger("Color", 2);
            }
            //to red
            if (trafficanimator.GetInteger("Color") == 2)
            {
                yield return new WaitForSeconds(3);
                trafficanimator.SetInteger("Color", 3);
                yield return new WaitForSeconds(0.5f);
                trafficanimator.SetInteger("Color", 0);
            }

            StartCoroutine(startlight());
        }

        if (ismanytraffic)
        {
            //to green
            if (trafficanimator.GetInteger("Color") == 0)
            {
                yield return new WaitForSeconds(traffictogreen);
                trafficanimator.SetInteger("Color", 1);
            }

            yield return new WaitForSeconds(0.5f);
            GameObject.Find(trafficname).GetComponent<TrafficLight>().isCounting = true;
            //Debug.Log(GameObject.Find(trafficname).GetComponent<TrafficLight>().name);

            //to yellow
            if (trafficanimator.GetInteger("Color") == 1)
            {
                yield return new WaitForSeconds(traffictoyellow);
                trafficanimator.SetInteger("Color", 2);
            }

            //to red
            if (trafficanimator.GetInteger("Color") == 2)
            {
                yield return new WaitForSeconds(2);
                trafficanimator.SetInteger("Color", 3);
                yield return new WaitForSeconds(0.5f);
                trafficanimator.SetInteger("Color", 0);
            }

            triggeronce = false;
            isCounting = false;
        }
    }
}
