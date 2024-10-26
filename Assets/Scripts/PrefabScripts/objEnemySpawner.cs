using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objEnemySpawner : MonoBehaviour
{
    public GameObject objEnemy;

    float maxSpawnRateInSeconds = 2f;

    void Start()

    {
        Invoke("SpawnEnemy", maxSpawnRateInSeconds);
        InvokeRepeating("IncreaseSpawnRate", 0f, 6f);
    }


    void SpawnEnemy()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        GameObject anEnemy = (GameObject)Instantiate(objEnemy);
        anEnemy.transform.position = new Vector2(Random.Range(min.x * 0.9f, max.x * 0.9f), max.y);

        ScheduleNextEnemySpawn();
    }



    void ScheduleNextEnemySpawn()
    {

        float spawnInNSeconds;

        if (maxSpawnRateInSeconds > 1f)

        {
            spawnInNSeconds = Random.Range(1f, maxSpawnRateInSeconds);
        }

        else{
            spawnInNSeconds = 1f;
        }
            
        Invoke("SpawnEnemy", spawnInNSeconds);

    }
    
    void IncreaseSpawnRate()
    {

        if (maxSpawnRateInSeconds > 1f){
            maxSpawnRateInSeconds--;
        }

        if (maxSpawnRateInSeconds == 1f){
            CancelInvoke("IncreaseSpawnRate");
        } 

    }

}
