using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isocar_controller : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 carLocation;  //car center location
    Vector2 velocity;
    Vector2 carForward;


    private float steerAngle;
    private Vector2 acceleration;

    float braking = -5f;
    float max_speed_reverse = 5f;

    private float  steering_angle = 25f;
    private float wheelBase = 1.5f;  //wheel base distance 
    private float friction = -0.1f;
    private float drag = -0.01f;
    private float enginePower = 5.0f;

    Rigidbody2D rb2d;
    isometricCarRenderer scriptrenderer;

    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        scriptrenderer = this.GetComponentInChildren<isometricCarRenderer>();
        carLocation = this.transform.position;
        velocity = Vector2.zero;
        carForward = Vector2.right;        
    }
        
    // Update is called once per frame
    void Update()
    {
        // Debug.Log(velocity);
        acceleration = Vector2.zero;
        get_input();
        apply_friction();
        calculate_steering(Time.deltaTime);
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        rb2d.velocity = velocity;
        scriptrenderer.setDirection(carForward);
    }

    private void get_input()
    {
        float turn = 0;
        if (Input.GetKey(KeyCode.RightArrow))
            turn -=1 ;
        else if (Input.GetKey(KeyCode.LeftArrow))
            turn += 1;
        steerAngle = turn * steering_angle;

        //accelerate
        if (Input.GetKey(KeyCode.UpArrow)) 
            acceleration = carForward * enginePower;
        else if (Input.GetKey(KeyCode.DownArrow)) 
            acceleration = carForward * braking;
    }

    Vector2 Rotate(Vector2 aPoint, float aDegree)
    {
        return Quaternion.Euler(0,0,aDegree) * aPoint;
    }

    private void calculate_steering(float delta) {
        velocity += acceleration * delta;
       
        Vector2 wheelLen = carForward*wheelBase / 2;
        // print(wheelBase * carForward);
        // Vector2 carHeadingAngle = new Vector2(Mathf.Cos(carHeading), Mathf.Sin(carHeading));
        Vector2 frontWheel = new Vector2(transform.position.x, transform.position.y) + wheelLen  ;
        Vector2 rearWheel = new Vector2(transform.position.x, transform.position.y) - wheelLen;

        rearWheel += delta * velocity;
        velocity = Rotate(velocity, steerAngle);
        frontWheel += velocity * delta;

        // debug line
        Vector3 ls = Vector3.zero;
        Debug.DrawLine( transform.position, ls=frontWheel, Color.green);
        Debug.DrawLine( transform.position, ls=rearWheel, Color.red);

        carForward = frontWheel - rearWheel;

        // carHeading = Mathf.Atan2(newHeading.y, newHeading.x);
        carForward.Normalize();
        // transform.position = new_position; 

        float d = Vector2.Dot(carForward, velocity);

        if(d < 0){
            velocity = -carForward * Mathf.Min(velocity.magnitude, max_speed_reverse);
        }
        else if(d>0){
            velocity = carForward * velocity.magnitude;
        }
        
    }

    private void apply_friction()
    {
        float v_magnitude = velocity.magnitude;
        if (v_magnitude < 0.1f)
            velocity = Vector2.zero;

        Vector2 friction_force = velocity * friction;
        Vector2 drag_force = velocity * v_magnitude * drag;

        if(v_magnitude < 100f)
            friction_force *= 3;

        acceleration += drag_force + friction_force;
    }

    void move_and_slide()
    {
        Debug.Log(velocity);
        rb2d.velocity = velocity;
    }
}
