using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObtacles : MonoBehaviour
{
    private float spawnPosZ = 10.5f;
    private float spawnPosY = 46;
    private float startDelay = 1f;
    private float startInterval = 0.5f;

    public float spawnPosX;
    public GameObject[] enemyToSpawn;

    void Start()
    {
        InvokeRepeating("SpawnEnemyAtPosition", startDelay, startInterval);
    }

    void SpawnEnemyAtPosition()
    {
        Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, spawnPosZ);

        Instantiate(enemyToSpawn[0], spawnPos, enemyToSpawn[0].transform.rotation);
    }
}
