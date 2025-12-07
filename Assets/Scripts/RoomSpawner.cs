using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class RoomSpawner : MonoBehaviour
{
    [Header("Zombie Spawning")]
    public SkeletonRoundManager skeletonRoundManager;

    [Header("Player")]
    public GameObject player;

    [Header("Room Generation Logic")]
    public Room StartRoom;
    public List<Room> roomPrefabs;
    public int maxRooms = 10;
    public int maxAttempts = 5;
    public int wallBuyCount = 1;

    private int currentAttempts = 0;
    private int spawnedRoomsCount = 0;

    public List<Room> spawnedRooms = new();
    public List<RoomExit> unusedExits = new();
    public List<RoomExit> exitsUsed = new();

    public List<WallBuy> possibleWallBuys = new();
    private List<Transform> wallBuyLocations = new();
    public List<EnemySpawnPoints> allEnemySpawnPoints = new();

    public NavMeshSurface globalNavMeshSurface;

    private void Awake()
    {
        skeletonRoundManager = FindFirstObjectByType<SkeletonRoundManager>();
    }

    private void Start()
    {
        StartCoroutine(GenerateLevelRoutine());
    }

    /// <summary>
    /// Full async generation pipeline
    /// </summary>
    private IEnumerator GenerateLevelRoutine()
    {
        // Disable all NavMeshAgents until generation completes
        DisableAllAgents();

        // Generate rooms
        GenerateRooms();

        // Close all exits
        foreach (RoomExit exit in unusedExits) exit.CloseExit();
        foreach (RoomExit exit in exitsUsed) exit.CloseExit();

        // Wait a frame so all room transforms settle
        yield return null;

        // Build the NavMesh
        globalNavMeshSurface.BuildNavMesh();

        // Wait one frame to ensure NavMesh jobs finish
        yield return new WaitForEndOfFrame();

        // Re-enable NavMeshAgents
        EnableAllAgents();

        // Spawn wall buys
        for (int i = 0; i < wallBuyCount; i++)
            SpawnRandomWallBuy();

        // Start enemy spawning
        skeletonRoundManager.startEnemySpawning(allEnemySpawnPoints);
    }

    /// <summary>
    /// Generates rooms and collects exits, wall buys, and enemy spawn points
    /// </summary>
    private void GenerateRooms()
    {
        // --- Start Room ---
        Room startRoom = InstantiateRoom(StartRoom, Vector3.zero, Quaternion.identity);
        spawnedRooms.Add(startRoom);
        unusedExits.AddRange(startRoom.GetExits());
        wallBuyLocations.AddRange(startRoom.GetWallBuyLocations());
        spawnedRoomsCount++;

        // --- Procedural Generation Loop ---
        while (spawnedRoomsCount < maxRooms && unusedExits.Count > 0 && currentAttempts < maxAttempts)
        {
            RoomExit exitToConnect = PickRandomExit();
            Room prefabRoom = PickCompatibleRoom(exitToConnect);

            if (prefabRoom == null)
                continue;

            Room newRoom = SpawnRoomAtExit(prefabRoom, exitToConnect);
            currentAttempts++;

            if (newRoom == null || CheckOverlap(newRoom))
            {
                Destroy(newRoom?.gameObject);
                continue;
            }

            // Add room to lists
            spawnedRooms.Add(newRoom);
            exitToConnect.collisionIsEntryWay = true;
            exitToConnect.CloseExit();
            spawnedRoomsCount++;

            unusedExits.AddRange(newRoom.GetExits());
            allEnemySpawnPoints.AddRange(newRoom.GetEnemySpawnPoints());
            wallBuyLocations.AddRange(newRoom.GetWallBuyLocations());

            // Move exits from unused → used
            unusedExits.Remove(exitToConnect);
            exitsUsed.Add(exitToConnect);
        }
    }

    #region Helper Methods

    private Room InstantiateRoom(Room prefab, Vector3 position, Quaternion rotation)
    {
        Room instance = Instantiate(prefab, position, rotation, transform);
        instance.name = prefab.name + "_Clone";
        instance.generationOrder = spawnedRoomsCount + 1;
        return instance;
    }

    private RoomExit PickRandomExit()
    {
        int index = Random.Range(0, unusedExits.Count);
        return unusedExits[index];
    }

    private Room PickCompatibleRoom(RoomExit existingExit)
    {
        List<Room> compatibleRooms = new List<Room>();

        foreach (Room room in roomPrefabs)
        {
            foreach (RoomType type in existingExit.roomType)
            {
                if (room.roomType == type)
                {
                    compatibleRooms.Add(room);
                    break;
                }
            }
        }

        if (compatibleRooms.Count == 0) return null;
        return compatibleRooms[Random.Range(0, compatibleRooms.Count)];
    }

    private Room SpawnRoomAtExit(Room prefab, RoomExit exitToConnect)
    {
        Quaternion rotation = Quaternion.LookRotation(exitToConnect.transform.forward, Vector3.up);
        Vector3 spawnPosition = exitToConnect.transform.position;
        return InstantiateRoom(prefab, spawnPosition, rotation);
    }

    private bool CheckOverlap(Room newRoom)
    {
        BoxCollider[] colliders = newRoom.GetComponentsInChildren<BoxCollider>();

        foreach (var col in colliders)
        {
            Collider[] overlaps = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("GeneratedRoom"));
            foreach (var hit in overlaps)
            {
                Room otherRoom = hit.GetComponentInParent<Room>();
                if (otherRoom != null && otherRoom != newRoom && newRoom.generationOrder > otherRoom.generationOrder)
                    return true;
            }
        }

        return false;
    }

    private void SpawnRandomWallBuy()
    {
        if (possibleWallBuys.Count == 0 || wallBuyLocations.Count == 0)
        {
            Debug.LogWarning("No wall buy prefabs or locations assigned!");
            return;
        }

        WallBuy prefab = possibleWallBuys[Random.Range(0, possibleWallBuys.Count)];
        Transform location = wallBuyLocations[Random.Range(0, wallBuyLocations.Count)];

        WallBuy newWallBuy = Instantiate(prefab, location.position, location.rotation);
        wallBuyLocations.Remove(location);
    }

    private void DisableAllAgents()
    {
        foreach (var agent in FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None))
            agent.enabled = false;
    }

    private void EnableAllAgents()
    {
        foreach (var agent in FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None))
            agent.enabled = true;
    }

    #endregion
}