using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRoundManager : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject zombiePrefab;

    [Header("Round Settings")]
    public int startingZombies = 5;
    public float spawnInterval = 1f;
    public int round = 1;
    public float roundDelay = 5f;

    private int zombiesToSpawn;   // how many still need to be spawned
    private int zombiesAlive;     // how many are alive in the scene
    private bool roundActive;

    private List<EnemySpawnPoints> allEnemySpawnPoints;

    public void startEnemySpawning(List<EnemySpawnPoints> enemySpawnPositionList)
    {
        allEnemySpawnPoints = enemySpawnPositionList;
        StartCoroutine(StartNextRound());
    }

    IEnumerator StartNextRound()
    {
        yield return new WaitForSeconds(roundDelay);

        roundActive = true;

        // HOW MANY WILL SPAWN THIS ROUND
        zombiesToSpawn = startingZombies + (round - 1) * 2;
        // RESET alive count
        zombiesAlive = 0;

        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        while (zombiesToSpawn > 0)
        {
            Transform spawn = allEnemySpawnPoints[Random.Range(0, allEnemySpawnPoints.Count)].transform;

            Instantiate(zombiePrefab, spawn.position, spawn.rotation);

            zombiesToSpawn--;
            zombiesAlive++;   // track alive zombies

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void OnZombieKilled()
    {
        zombiesAlive--;

        Debug.Log($"Zombie killed. Alive: {zombiesAlive}");

        // When all zombies that were spawned are dead → new round
        if (zombiesAlive <= 0 && roundActive)
        {
            roundActive = false;
            round++;

            Debug.Log($"Starting round {round}");
            StartCoroutine(StartNextRound());
        }
    }
}
