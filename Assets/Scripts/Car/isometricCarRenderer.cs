using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCarRenderer : MonoBehaviour
{
    private readonly string[] staticAnimations = {"car_8", "car_7", "car_6", "car_5", "car_4", "car_3", "car_2", "car_1",
                                                    "car_9", "car_10", "car_11", "car_12", "car_13", "car_14", "car_15"};

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
        lastAnimationIndex = directionToIndex(direction, 28);

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
