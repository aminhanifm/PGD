using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarColor : MonoBehaviour
{
    private float randomcolor;
    private Renderer carmaterial;


    void Start()
    {
        randomcolor = Random.Range(0.1f, 1f);
        Debug.Log(randomcolor);


        carmaterial = gameObject.GetComponent<Renderer>();
        carmaterial.material.SetVector("_HSVAAdjust", new Vector4(randomcolor,0,0,0));

    }

    void Update()
    {
        //for test only
        if (Input.GetKeyDown(KeyCode.Space))
        {
            randomcolor += 0.01f;
            Debug.Log(randomcolor);
            carmaterial.material.SetVector("_HSVAAdjust", new Vector4(randomcolor, 0, 0, 0));
        }
    }
}
