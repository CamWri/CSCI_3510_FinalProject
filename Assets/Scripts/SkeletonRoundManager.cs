using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonRoundManager : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject skeletonPrefab;

    [Header("Round Settings")]
    public int startingSkeletons = 5;
    public float spawnInterval = 1f;
    public int round = 1;
    public float roundDelay = 5f;

    private int skeletonsToSpawn;   // how many still need to be spawned
    private int skeletonsAlive;     // how many are alive in the scene
    private bool roundActive;

    private List<EnemySpawnPoints> allEnemySpawnPoints;

    public void startEnemySpawning(List<EnemySpawnPoints> enemySpawnPositionList)
    {
        allEnemySpawnPoints = enemySpawnPositionList;
        HUDController.Instance.UpdateRoundText(round.ToString());
        StartCoroutine(StartNextRound());
    }

    IEnumerator StartNextRound()
    {
        yield return new WaitForSeconds(roundDelay);

        roundActive = true;

        // HOW MANY WILL SPAWN THIS ROUND
        skeletonsToSpawn = startingSkeletons + (round - 1) * 2;
        // RESET alive count
        skeletonsAlive = 0;

        StartCoroutine(SpawnSkeletons());
    }

    IEnumerator SpawnSkeletons()
    {
        while (skeletonsToSpawn > 0)
        {
            Transform spawn = allEnemySpawnPoints[Random.Range(0, allEnemySpawnPoints.Count)].transform;

            GameObject skeletonObj = Instantiate(skeletonPrefab, spawn.position, spawn.rotation);

            // Apply scaling
            Skeleton skeleton = skeletonObj.GetComponent<Skeleton>();
            if (skeleton != null)
            {
                skeleton.UpdateStats(FloorManager.Instance.currentFloor, round);
                Debug.Log(skeleton.health);
                Debug.Log(skeleton.damage);
            }

            skeletonsToSpawn--;
            skeletonsAlive++;   // track alive skeletons

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void OnSkeletonKilled()
    {
        skeletonsAlive--;

        Debug.Log($"Skeleton killed. Alive: {skeletonsAlive}");

        // When all skeletons that were spawned are dead → new round
        if (skeletonsAlive <= 0 && roundActive)
        {
            roundActive = false;
            round++;
            HUDController.Instance.UpdateRoundText(round.ToString());

            Debug.Log($"Starting round {round}");
            StartCoroutine(StartNextRound());
        }
    }
}
