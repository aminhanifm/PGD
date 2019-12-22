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
    private new IsometricCarRenderer renderer;

    int cur = 0;
    float cast_forward_len = 10f;
    float lastDistRelative = Mathf.Infinity;
    float maintain_velocity = 0;
    bool needToMaintain = false;

    protected override void Start()
    {
        base.Start();
        renderer = this.GetComponent<IsometricCarRenderer>();
        braking = -2f;
        max_speed_reverse = 5f;
        steering_angle = 45f;
        wheelBase = 1.5f;         //wheel base distance 
        friction = -0.1f;
        drag = -0.01f;
        enginePower = 2.0f;
        target = (Vector2)transform.position;
    }

    protected override void Update()
    {
        float distance = (target - (Vector2)transform.position).magnitude;
        if (distance < wheelBase / 2)
        {
            cur = (cur + 1)% locs.Length;
            target = locs[cur];
        }
        //print("target: " + target);
        acceleration = Vector2.zero;
        SteerControl();
        //ObserveEnvirontment();   
        //DriveForward();
        tesajah();
        base.Update();
        renderer.setDirection(carForward);
    }

    void tesajah()
    {
        Vector2 st = Vector2.zero;
        st = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(st, carForward, cast_forward_len);
        if (hit.collider != null)
        {
            //ambil jarak sekarang
            float distRelative = Vector3.Distance(hit.transform.position, transform.position) - wheelBase*2;

            //cari jarak minim
            float minimumDist = Vector2.SqrMagnitude(velocity) / (-1 * 2 * braking);

            print("relative: "+distRelative+"    minimum: "+minimumDist);

            if (distRelative <= minimumDist) DriveBackward();
            else DriveForward();
        }
        else
        {
            DriveForward();
        }
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

        //print(needToMaintain +" "+ maintain_velocity);
        if (needToMaintain)
            velocity = maintain_velocity * carForward;
            //velocity = Mathf.Min(velocity.magnitude, maintain_velocity) * carForward;
    }

    protected override void FixedUpdate()
    {
        
        base.FixedUpdate();
    }

    void DriveForward()
    {
        acceleration = carForward * enginePower;
    }

    void DriveBackward()
    {
        acceleration = carForward * braking;
    }

    void ObserveEnvirontment()
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

        if (myangle < -1)
            turn += 1;
        else if (myangle > 1)
            turn -= 1;

        steerAngle = turn * steering_angle;
    }

//#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        HandleUtility.Repaint();

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        
        
        for (int i = 0; i < locs.Length; i++)
        {
            int next = (i + 1) % locs.Length;
            Gizmos.DrawLine(locs[i], locs[next]);

            Gizmos.DrawSphere(locs[i], 1);
        }
        
        Gizmos.color = Color.green;
    }
    //#endif
}
