using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public Points points;
    public GameObject carAi;

    public int max_car;
    private int pointSize;
    private int cur_car;

    float timeToSpawn = 0;

    //private List<GameObject> cars;

    // Start is called before the first frame update
    void Awake()
    {
        //cars = new List<GameObject>();
        pointSize = points.getPointsCount();
        cur_car = 0;
        timeToSpawn = 0;
        //randomSpawnerAtStart();

        //foreach (Transform child in this.transform)
        //{
        //    //print(child.name);
        //    cars.Add(child.gameObject);
        //}
        //max_car = cars.Count;
        //StartCoroutine(spawnSequentially());
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

    IEnumerator spawnSequentially()
    {
        while (cur_car < max_car)
        {
            spawnCar();
            yield return new WaitForSeconds(4);
        }
        
    }

    //IEnumerator initSequentially()
    //{
    //    while (cur_car < max_car)
    //    {
    //        //int idx = Random.Range(0, pointSize - 1);
    //        cars[cur_car].GetComponent<CarPointsManager>().init(0);
    //        cars[cur_car].GetComponent<IsoCarAI>().aktif = true;
    //        cur_car++;
    //        yield return new WaitForSeconds(3);
    //    }

    //}
}
