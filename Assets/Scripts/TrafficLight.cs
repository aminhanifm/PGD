using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum Orientation { DownLeft, DownRight, UpLeft, UpRight }


public class TrafficLight : MonoBehaviour
{

    [HideInInspector] public int traffictoyellow;
    [HideInInspector] public int traffictogreen;
    [HideInInspector] public Animator trafficanimator;
    public Transform colliderObject;
    public int type; //set type from 1 to 4 which has time differences
    public string trafficname; //only for 2 or more traffics
    public bool isCounting; //must set to yes
    public bool ismanytraffic; //set yes if need to compare with other traffic light
    private bool triggeronce;
    public int firstcondition; // set 1 to make it start animating

    public Orientation orientation;
    [HideInInspector] public Vector2 orientationDir;
    private CircleCollider2D coll;
    [HideInInspector]public bool isStop = true;
    [HideInInspector]public bool isStart = false;

    private trafficAnimatorList tal;

    void Start()
    {
        coll = colliderObject.GetComponent<CircleCollider2D>();
        trafficanimator = gameObject.GetComponent<Animator>();
        tal = gameObject.GetComponentInParent<trafficAnimatorList>();

        switch (orientation)
        {
            case Orientation.DownLeft:
                orientationDir = new Vector2(2, 1).normalized;
                trafficanimator.runtimeAnimatorController = tal.downLeftAnimator as RuntimeAnimatorController;
                break;
            case Orientation.DownRight:
                orientationDir = new Vector2(-2, 1).normalized;
                trafficanimator.runtimeAnimatorController = tal.downRightAnimator as RuntimeAnimatorController;
                break;
            case Orientation.UpLeft:
                orientationDir = - new Vector2(-2, 1).normalized;
                trafficanimator.runtimeAnimatorController = tal.upLeftAnimator as RuntimeAnimatorController;
                break;
            case Orientation.UpRight:
                orientationDir = - new Vector2(2, 1).normalized;
                trafficanimator.runtimeAnimatorController = tal.upRightAnimator as RuntimeAnimatorController;
                break;
            default:
                break;
        }

        if(type == 1)
        {
            type1();
        }
        else if (type == 2)
        {
            type2();
        }
        else if (type == 3)
        {
            type3();
        }
        else if (type == 4)
        {
            type4();
        }

    }

    void Update()
    {
        //testing purpose only
        //if (Input.GetButtonDown("Jump"))
        //{
        //    trafficanimator.SetInteger("Color", 1);
        //    StartCoroutine(startlight());
        //    //Debug.Log("Succed");
        //}
    }

    private void FixedUpdate()
    {
        //if (isStart)
        //{
        //    if (isCounting && !triggeronce)
        //    {
        //        StartCoroutine(startlight());
        //        triggeronce = true;
        //        //Debug.Log("Succed");
        //    }
        //}
    }

    public void setToGreenDuration(int dur)
    {
        traffictogreen = dur;
    }

    public void setToYellowDuration(int dur)
    {
        traffictoyellow = dur;
    }

    public int getToYellowDuration()
    {
        return traffictoyellow;
    }

    public void setAnimatorAt(int cond)
    {
        trafficanimator.SetInteger("Color",cond);
        StartCoroutine(startlight());
    }

    public void stopAnimator()
    {
        StopAllCoroutines();
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
                coll.enabled = true;
                yield return new WaitForSeconds(traffictogreen);
                trafficanimator.SetInteger("Color", 1);
            }
            //to yellow
            if (trafficanimator.GetInteger("Color") == 1)
            {
                coll.enabled = false;
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
                isStop = true;
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

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        //HandleUtility.Repaint();

        //Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(colliderObject.transform.position, 1);
        Gizmos.DrawLine(transform.position, colliderObject.transform.position);
    }
#endif
}
