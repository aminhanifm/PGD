using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class IsoCarAI : CarGeneric
{
    public Vector2[] locs;
    private Vector2 target;
    private List<Vector2> tempTarget;
    private new IsometricCarRenderer renderer;

    float cast_forward_len = 10f;
    float lastDistRelative = Mathf.Infinity;
    float maintain_velocity = 0;
    bool needToMaintain = false;

    private int lane;            //in which lane this car will drive , start from 0 for the left most

    private CarPointsManager carPM;

    private float maxVelocity;

    protected override void Start()
    {
        base.Start();
        renderer = this.GetComponent<IsometricCarRenderer>();
        braking = -4f;
        max_speed_reverse = 5f;
        steering_angle = 65f;
        wheelBase = 1.5f;         //wheel base distance 
        friction = -0.1f;
        drag = -0.01f;
        enginePower = 2f;

        carPM = this.gameObject.GetComponent<CarPointsManager>();
        maxVelocity = 5f;
        lane = 0;
        target = carPM.getCurLocation(lane);
        tempTarget = new List<Vector2>();

        carForward = carPM.getLocationDirection();
    }


    protected override void Update()
    {

        if (tempTarget.Count > 0)
        {
            target = tempTarget[0];
        }

        float distance = (target - (Vector2)transform.position).magnitude;

        //print(Mathf.FloorToInt(distance));
        if (Mathf.FloorToInt(distance) < wheelBase/2)
        {
            if (tempTarget.Count > 0)
            {
                tempTarget.RemoveAt(0);
                target = carPM.getCurLocation(lane);
            }
            else
            {
                carPM.getNext(lane);
                target = carPM.getCurLocation(lane);
            }
        }

        acceleration = Vector2.zero;
        SteerControl();
        ObserveEnvirontment();
        
        base.Update();
        renderer.setDirection(carForward);
    }

    void changeCarLane()
    {
        int laneDir;
        int maxLane = carPM.getMaxLane();

        if (maxLane <= 1) return;
        else if (lane == 0) laneDir = 1;
        else if (lane == maxLane - 1) laneDir = -1;
        else laneDir = 1;

        lane += laneDir;
        Vector2 vDir = carPM.getLocationDirection();

        float Dist = Vector2.SqrMagnitude(velocity) / (-1 * 2 * braking);

        Vector2 laneTarget = rb2d.position + vDir * Dist - laneDir * Vector2.Perpendicular(vDir) * carPM.getLaneWidth();
        tempTarget.Add(laneTarget);
    }

    void ObserveEnvirontment()
    {
        Vector2 st = Vector2.zero;
        st = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(st, carForward, cast_forward_len);
        if (hit.collider != null)
        {
            string excludeName = hit.collider.gameObject.name;
            if (excludeName=="KiriAI" || excludeName == "KananAI" || excludeName == "DepanAI" || excludeName == "BelakangAI")
            {
                DriveForward();
            }

            //ambil jarak sekarang
            float distRelative = Vector3.Distance(hit.transform.position, transform.position) - wheelBase * 1.5f;

            //cari jarak minim
            float minimumDist = Vector2.SqrMagnitude(velocity) / (-1 * 2 * braking);

            if (distRelative <= minimumDist)
            {
                if (hit.transform.tag == "car")
                {
                    DriveBackward();
                }
                else if (hit.transform.tag == "traffic")
                {
                    DriveBackward();
                }
                return;
            }
            else DriveForward();
        }
        else
        {
            DriveForward();
        }

        //Vector2 mDir = carPM.getRawNextLocationDir();
        //if (mDir == Vector2.zero) DriveForward();
        //else
        //{
        //    int mAngle = (int) Vector2.SignedAngle(carForward, mDir);
        //    mAngle = Mathf.Abs(mAngle);
        //    Vector2 vDir = carPM.getRawCurLocation();

        //    //print(mAngle);

        //    //ambil jarak sekarang
        //    float distRelative = Vector3.Distance(new Vector3(vDir.x, vDir.y, 0), transform.position) - wheelBase * 1.5f;

        //    //cari jarak minim
        //    float minimumDist = Vector2.SqrMagnitude(velocity) / (-1 * 2 * braking *2);

        //    if (distRelative < minimumDist || mAngle>4)
        //    {
        //        if(velocity.magnitude > 10f)
        //        {
        //            print("aye");
        //            DriveBackward();
        //        }
        //        else
        //        {
        //            print("aye2");
        //            DriveForward();
        //        }

        //    }
        //    else
        //    {
        //        DriveForward();
        //    }
        //}
    }

    protected override void calculate_steering(float delta)
    {
        base.calculate_steering(delta);

        float d = Vector2.Dot(carForward, rb2d.velocity);
        float s = Vector2.Dot(carForward, velocity);

        if (s < 0)
        {
            if (d <= 0)
            {
                velocity = carForward * 0;
                rb2d.velocity = Vector2.zero;
            }
            else
            {
                velocity = -carForward * Mathf.Min(velocity.magnitude, max_speed_reverse);
            }
        }
        else if (s >= 0)
        {
            velocity = carForward * velocity.magnitude;
        }

        if (velocity.magnitude > maxVelocity) velocity = carForward*maxVelocity;
    }

    protected override void FixedUpdate()
    {
        
        base.FixedUpdate();
    }

    void DriveForward(float factor=1)
    {
        acceleration = carForward * enginePower * factor;
        //print(acceleration);
    }

    void DriveBackward()
    {
        acceleration = carForward * braking;
    }

    void tesajah()
    {
        //needToMaintain = false;

        //check if there is a car ahead
        Vector2 st = Vector2.zero;
        st = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(st, carForward, cast_forward_len);
        if(hit.collider != null)
        {
            float distRelative = Vector3.Distance(hit.transform.position, transform.position);

            //print("last: " + lastDistRelative + "  now: "+ distRelative+ "  time: "+ Time.deltaTime);
            if (distRelative < lastDistRelative && rb2d.velocity!= Vector2.zero)
            {
                needToMaintain = false;
                DriveBackward();        //decelerate
            }
            else if(distRelative > lastDistRelative)
            {
                needToMaintain = false;
                DriveForward();
            }
            else
            {
                if(needToMaintain ==false)
                    maintain_velocity = velocity.magnitude;
                needToMaintain = true;
            }

            lastDistRelative = distRelative;

        }
        else
        {
            DriveForward();
        }

        //print(needToMaintain+"  maintain: "+maintain_velocity);
    }

    void SteerControl()
    {
        float turn = 0;

        Vector2 myTarget = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized;
        float myangle = Vector2.SignedAngle(myTarget, carForward);

        //print(myangle);

        if (myangle < -4)
            turn += Mathf.Abs(myangle) / steering_angle;
        else if (myangle > 4)
            turn -= Mathf.Abs(myangle) / steering_angle;

        steerAngle = turn * steering_angle;
        steerAngle = Mathf.Min(steerAngle, 80);
    }
}
