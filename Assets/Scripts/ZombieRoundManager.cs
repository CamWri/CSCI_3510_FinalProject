using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRoundManager : MonoBehaviour
{
    public RoomSpawner roomSpawner;

    [Header("Spawning Settings")]
    public GameObject zombiePrefab;


    [Header("Round Settings")]
    public int startingZombies = 5;
    public float spawnInterval = 1f; // seconds between zombies
    public int round = 1;
    public float roundDelay = 5f;

    private int zombiesRemaining;
    private bool roundActive;

    void Start()
    {
        StartCoroutine(StartNextRound());
    }

    IEnumerator StartNextRound()
    {
        yield return new WaitForSeconds(roundDelay);
        roundActive = true;

        // Determine zombies to spawn based on round
        zombiesRemaining = startingZombies + (round - 1) * 2;

        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        while (zombiesRemaining > 0)
        {
            //Get the list from 
            Transform spawn = roomSpawner.allEnemySpawnPoints[Random.Range(0, roomSpawner.allEnemySpawnPoints.Count)].transform;
            Instantiate(zombiePrefab, spawn.position, spawn.rotation);
            zombiesRemaining--;

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void OnZombieKilled()
    {
        zombiesRemaining--;
        if (zombiesRemaining <= 0 && roundActive)
        {
            roundActive = false;
            round++;
            StartCoroutine(StartNextRound());
        }
    }
}
