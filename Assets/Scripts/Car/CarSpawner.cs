using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public Points points;
    public GameObject carAi;

    [SerializeField] private int max_car;
    private int pointSize;
    public int cur_car;

    // Start is called before the first frame update
    void Start()
    {
        pointSize = points.getPointsCount();
        cur_car = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
