using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isocar_controller : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 carLocation;  //car center location
    float carHeading;
    float steerAngle;
    Vector2 velocity;


    private float steering_angle;
    private Vector2 acceleration;

    [SerializeField] private float carSpeed = 1.0f;
    [SerializeField]  private float wheelBase = 70f;  //wheel base distance 
    [SerializeField] private float friction = -0.9f;
    [SerializeField] private float drag = -0.0015f;
    [SerializeField] private float enginePower = 10.0f;

    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        carLocation = this.transform.position;
        velocity = Vector2.zero;
        steering_angle = 15f; // amount of wheel to turn
        carHeading = 0f;        // in degree 0-360
    }
        
    // Update is called once per frame
    void Update()
    {
        Debug.Log(velocity);
        acceleration = Vector2.zero;
        get_input();
        apply_friction();
        calculate_steering(Time.deltaTime);
        velocity += acceleration * Time.deltaTime;
        move_and_slide();
    }

    private void get_input()
    {
        float turn = 0;
        if (Input.GetKey(KeyCode.RightArrow))
            turn +=1 ;
        else if (Input.GetKey(KeyCode.LeftArrow))
            turn -= 1;
        steerAngle = turn * steering_angle;

        //accelerate
        if (Input.GetKey(KeyCode.UpArrow)) 
            acceleration = new Vector2(Mathf.Cos(carHeading), Mathf.Sin(carHeading)) * enginePower;
    }

    private void calculate_steering(float delta) {
        Vector2 carHeadingAngle = new Vector2(Mathf.Cos(carHeading), Mathf.Sin(carHeading));
        Vector2 frontWheel = carLocation + wheelBase / 2 * carHeadingAngle;
        Vector2 rearWheel = carLocation - wheelBase / 2 * carHeadingAngle;

        rearWheel += carSpeed * delta * carHeadingAngle;
        frontWheel += carSpeed * delta * new Vector2(Mathf.Cos(carHeading + steerAngle ), Mathf.Sin(carHeading + steerAngle));

        Vector2 newHeading = frontWheel - rearWheel ;
        newHeading.Normalize();

        carHeading = Mathf.Atan2(newHeading.y, newHeading.x);
        carLocation = (frontWheel - rearWheel) / 2;
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
        rb2d.velocity = velocity;
    }
}
