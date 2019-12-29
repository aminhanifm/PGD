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

    float timeToSpawn = 0;

    // Start is called before the first frame update
    void Awake()
    {
        pointSize = points.getPointsCount();
        cur_car = 0;
        timeToSpawn = 0;
        randomSpawnerAtStart();
    }

    // Update is called once per frame
    void Update()
    {
        //if (timeToSpawn > 0) timeToSpawn -= Time.deltaTime;

        //else if (cur_car < max_car)
        //{
        //    int idx = Random.Range(0, pointSize - 1);
        //    spawnCar(idx);
        //    timeToSpawn = 3f;
        //}
    }

    void spawnCar(int idx = -1)
    {
        if (idx == -1)
        {
            idx = Random.Range(0, pointSize - 1);
        }

        GameObject carAI_new = Instantiate(carAi);
        carAI_new.GetComponent<CarPointsManager>().init(idx);
        cur_car++;
    }

    void randomSpawnerAtStart()
    {
        int n = 25;
        for (var i = 0; i < n; i++)
        {
            int idx = Random.Range(0, pointSize - 1);
            spawnCar(idx);
        }
    }
}
