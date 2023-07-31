using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemies;
    private float spawnInterval = 5f;

    private Transform[] spawnPoints;

    private void Start()
    {
        // Get the spawn points as children of the GameManager
        spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }

        StartCoroutine(UpdateSpawnInterval());
    }

    private IEnumerator UpdateSpawnInterval()
    {
        DayAndNightControl dayNightControl = FindObjectOfType<DayAndNightControl>();
        while (true)
        {
            SpawnRandomEnemy();

            // Wait for some time before checking again
            float timer = 0f;

            while (timer < spawnInterval)
            {
                spawnInterval = dayNightControl.SecondsInAFullDay / 3f;

                if (Time.timeScale != 1f)
                {
                    spawnInterval /= 100f;
                }

                yield return new WaitForEndOfFrame();

                timer += Time.deltaTime;
            }
        }
    }

    private void SpawnRandomEnemy()
    {
        Debug.Log("Spawn!");

        // Randomly select an enemy and a spawn point
        GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Calculate the random rotation with z-axis rotation being random
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        // Instantiate the selected enemy at the chosen spawn point with the calculated rotation
        Instantiate(randomEnemy, randomSpawnPoint.position, randomRotation);
    }
}
