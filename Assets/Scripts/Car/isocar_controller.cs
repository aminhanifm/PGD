using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class isocar_controller : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Transform repairimg;
    private Savemanagement savemanagement;

    // Start is called before the first frame update
    private UIcontroller uicontroller;
    private SteeringLogic steerlogic;

    Vector2 carLocation;  //car center location
    [HideInInspector] public Vector2 velocity;
    [HideInInspector] public Vector2 carForward;

    private bool accelerating = false;
    private bool breaking = false;

    private float steerAngle;
    private Vector2 acceleration;
    private float steerturn;

    float braking = -1f;
    float max_speed_reverse = 5f;

    private float  steering_angle = 25f;
    private float wheelBase = 1.5f;  //wheel base distance 
    private float friction = -0.1f;
    private float drag = -0.01f;
    private float enginePower = 2f;


    [HideInInspector] public double curspeed;
    [HideInInspector] public int curspeedint;

    Rigidbody2D rb2d;
    IsometricCarRenderer scriptrenderer;

    private bool ishitten = false;

    void Start()
    {
        repairimg = GameObject.Find("Repair Icon").GetComponent<Transform>();
        uicontroller = FindObjectOfType(typeof(UIcontroller)) as UIcontroller;
        steerlogic = FindObjectOfType(typeof(SteeringLogic)) as SteeringLogic;
        savemanagement = FindObjectOfType(typeof(Savemanagement)) as Savemanagement;

        rb2d = this.GetComponent<Rigidbody2D>();
        scriptrenderer = this.GetComponentInChildren<IsometricCarRenderer>();
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
        if (breaking)
        {
            acceleration = carForward * braking;
        }
        if (accelerating)
        {
            acceleration = carForward * enginePower;
        }

        if (steerlogic.wheelAngle > 0)
        {
            steerturn = -1;
        }
        else if (steerlogic.wheelAngle < 0)
        {
            steerturn = 1;
        }
        if (!steerlogic.wheelBeingHeld)
        {
            steerturn = 0;
        }
        steerAngle = steerturn * steering_angle;

        apply_friction();
        calculate_steering(Time.deltaTime);
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        
        curspeed = rb2d.velocity.magnitude * 3.6;
        curspeedint = (int) curspeed;
        rb2d.velocity = velocity;
        scriptrenderer.setDirection(carForward);
        if (savemanagement.repair <= 30 && savemanagement.repair != 0)
        {
            steering_angle = 20f;
            enginePower = 1.2f;
        }
        else if (savemanagement.repair > 30)
        {
            steering_angle = 25f;
            enginePower = 2f;
        }
        if (savemanagement.repair <= 0)
        {
            steering_angle = 15f;
            enginePower = 1f;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.collider.CompareTag("Collisiontorepair") && !ishitten)
        {
            ishitten = true;
            
            if (savemanagement.repair > 0)
            {
                savemanagement.repair -= 1;
                repairimg.DOScale(1.3f, 0.25f);
                print(savemanagement.repair);
            }
            if(savemanagement.repair <= 0)
            {
                uicontroller.repairimg.DOColor(Color.black, 1f);
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Collisiontorepair") && ishitten)
        {
            ishitten = false;
            repairimg.DOScale(1f, 0.5f);
            print("exit");
        }
    }

    public void OnPointerDown(PointerEventData data)
    {

    }

    public void OnPointerUp(PointerEventData data)
    {

    }

    public void carsteeringup(BaseEventData up)
    {
        string name;

        name = up.selectedObject.name;

        if (name == "Accelerate")
        {
            accelerating = false;
        }

        else if (name == "Break")
        {
            breaking = false;
        }
    }

    public void carsteeringdown(BaseEventData input)
    {
        string name;

        name = input.selectedObject.name;

        if (uicontroller.UIcanvas.interactable == true && !uicontroller.iswanted)
        {
            if (uicontroller.fuelfill.fillAmount > 0)
            {
                if (name == "Accelerate")
                {
                    accelerating = true;
                }

                else if (name == "Break")
                {
                    breaking = true;
                }
            }

        }
    }

    private void get_input()
    {
        if (uicontroller.UIcanvas.interactable == true)
        {
            float turn = 0;
            if (Input.GetKey(KeyCode.RightArrow))
                turn -= 1;
            else if (Input.GetKey(KeyCode.LeftArrow))
                turn += 1;
            steerAngle = turn * steering_angle;
            if (uicontroller.fuelfill.fillAmount > 0)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                    acceleration = carForward * enginePower;
                else if (Input.GetKey(KeyCode.DownArrow))
                    acceleration = carForward * braking;
            }
        }
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

        if(d <= 0){
            velocity = -carForward * Mathf.Min(velocity.magnitude, max_speed_reverse);
        }
        else if(d>0){
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
