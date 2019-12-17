using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isometricCarRenderer : MonoBehaviour
{
    private readonly string[] staticDirections = {"car_8", "car_7", "car_6", "car_5", "car_4", "car_3", "car_2", "car_1",
                                                    "car_9", "car_10", "car_11", "car_12", "car_13", "car_14", "car_15"};
    int lastDirectionIndex;
    Animator animator;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    public void setDirection(Vector2 vDir){

    }

    private int directionToIndex(){

        return 1;
    }
}
