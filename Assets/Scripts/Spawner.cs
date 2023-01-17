using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemiesToSpawn;
    private float _timeToSpawn = 10f;
    public int numOfEnemies;
    public float distanceToSpawn = 10f;

    private Vector3 center;
    public PlayerController playerController;

    private void Start()
    {
        InvokeRepeating("SpawnEnemies", 2f, _timeToSpawn);
    }

    private void Update()
    {
        if (playerController == null)
        {
            return;
        }
        center = playerController.transform.position;
    }

    private void SpawnEnemies()
    {
        if(playerController == null) {
            return;
        }

        for (int i = 0; i < numOfEnemies; i++)
        {
            Vector3 pos = RandomCircle(center, distanceToSpawn);
            Instantiate(enemiesToSpawn[0], pos, Quaternion.identity);
        }
        numOfEnemies++;
    }

    private Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
}
