using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isometricCarRenderer : MonoBehaviour
{
    private readonly string[] staticAnimations = {"car_31", "car_30", "car_29", "car_28", "car_27", "car_26", "car_25",
                                                    "car_24", "car_23", "car_22", "car_21", "car_20", "car_19", "car_18", "car_17",
                                                    "car_16", "car_15", "car_14", "car_13", "car_12", "car_11", "car_10", "car_9",
                                                    "car_8", "car_7", "car_6", "car_5", "car_4", "car_3", "car_2", "car_1",
                                                    "car_32", "car_33", "car_34", "car_35", "car_36",
                                                    "car_37", "car_38", "car_39", "car_40", "car_41", "car_42", "car_43",
                                                    "car_44", "car_45", "car_46", "car_47", "car_48", "car_49", "car_50",
                                                    "car_51", "car_52", "car_53", "car_54", "car_55", "car_56", "car_57",
                                                    "car_58", "car_59", "car_60", "car_61"};

    string[] directionArray;
    int lastAnimationIndex;

    bool facingRight = true;
    Animator animator;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        animator = this.GetComponent<Animator>();
        directionArray = staticAnimations;
    }

    public void setDirection(Vector2 direction){
        lastAnimationIndex = directionToIndex(direction, 120);

        if(lastAnimationIndex > 0 && facingRight) flip();
        else if(lastAnimationIndex < 0 && !facingRight) flip();

        animator.Play(directionArray[Mathf.Abs(lastAnimationIndex)]);
    }

    void flip(){
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;

        scale.x *= -1;
        transform.localScale = scale;
    }

    private int directionToIndex(Vector2 direction, int sliceCount){
        Vector2 normDir = direction.normalized;

        float step = 360f/sliceCount;

        float halfStep = step/2;

        float angle = Vector2.SignedAngle(Vector2.up, normDir);

        angle += halfStep;

        float stepCount = angle/step;
        return Mathf.FloorToInt(stepCount);
    }
}
