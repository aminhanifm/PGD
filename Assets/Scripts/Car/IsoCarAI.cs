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

    protected override void Start()
    {
        base.Start();
        renderer = this.GetComponent<IsometricCarRenderer>();
        braking = -5f;
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
        print("target: " + target);
        acceleration = Vector2.zero;
        SteerControl();
        DriveForward();        
        base.Update();
        renderer.setDirection(carForward);
    }

    protected override void FixedUpdate()
    {
        
        base.FixedUpdate();
    }

    void DriveForward()
    {
        acceleration = carForward * enginePower;
    }

    void SteerControl()
    {
        float turn = 0;

        Vector2 myTarget = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized;
        float myangle = Vector2.SignedAngle(myTarget, carForward);

        print(myangle);

        if (myangle < -1)
            turn += 1;
        else if (myangle > 1)
            turn -= 1;

        steerAngle = turn * steering_angle;
    }

#if UNITY_EDITOR
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
    #endif
}
