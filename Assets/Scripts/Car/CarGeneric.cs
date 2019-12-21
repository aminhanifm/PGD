using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarGeneric : MonoBehaviour
{
    private Vector2 velocity;
    protected Vector2 carForward;
    protected Vector2 acceleration;
    protected float steerAngle;

    protected float braking = -5f;
    protected float max_speed_reverse = 5f;
    protected float steering_angle = 25f;
    protected float wheelBase = 1.5f;         //wheel base distance 
    protected float friction = -0.1f;
    protected float drag = -0.01f;
    protected float enginePower = 5.0f;

    private Rigidbody2D rb2d;

    protected virtual void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        
        velocity = Vector2.zero;
        carForward = Vector2.right;
    }


    protected virtual void Update()
    {
        apply_friction();
        calculate_steering(Time.deltaTime);
    }

    protected virtual void FixedUpdate() {
        rb2d.velocity = velocity;
    }

    protected virtual void OnDrawGizmos() { }

    Vector2 Rotate(Vector2 aPoint, float aDegree)
    {
        return Quaternion.Euler(0, 0, aDegree) * aPoint;
    }

    private void calculate_steering(float delta)
    {
        velocity += acceleration * delta;

        Vector2 wheelLen = carForward * wheelBase / 2;        
        Vector2 frontWheel = new Vector2(transform.position.x, transform.position.y) + wheelLen;
        Vector2 rearWheel = new Vector2(transform.position.x, transform.position.y) - wheelLen;

        rearWheel += delta * velocity;
        velocity = Rotate(velocity, steerAngle);
        frontWheel += velocity * delta;

        // debug line
        Vector3 ls = Vector3.zero;
        Debug.DrawLine(transform.position, ls = frontWheel, Color.green);
        Debug.DrawLine(transform.position, ls = rearWheel, Color.red);

        carForward = frontWheel - rearWheel;
        carForward.Normalize();

        float d = Vector2.Dot(carForward, velocity);

        if (d <= 0)
        {
            velocity = -carForward * Mathf.Min(velocity.magnitude, max_speed_reverse);
        }
        else if (d > 0)
        {
            velocity = carForward * velocity.magnitude;
        }

    }

    private void apply_friction()
    {
        float v_magnitude = velocity.magnitude;
        //if (v_magnitude < 0.1f)
        //    velocity = Vector2.zero;

        Vector2 friction_force = velocity * friction;
        Vector2 drag_force = velocity * v_magnitude * drag;

        if (v_magnitude < 100f)
            friction_force *= 3;

        acceleration += drag_force + friction_force;
    }
}
