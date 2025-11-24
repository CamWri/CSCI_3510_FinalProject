using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;

public class RoomSpawner : MonoBehaviour
{
    /*
        Future Notes:
            - For doors that can open and close, mark them as NavMeshObstacle, Add a NavMeshObstacle component with Carve = true, and Toggle enabled at runtime when the door opens/closes.
    */

    public GameObject player;

    public Room StartRoom;
    public List<Room> roomPrefabs;
    public int maxRooms = 10;
    public int maxAttempts = 5;
    private int currentAttempts = 0;

    private int spawnedRoomsCount;
    public List<Room> spawnedRooms = new();
    public List<RoomExit> openExits = new();
    public List<EnemySpawnPoints> allEnemySpawnPoints = new();

    public NavMeshSurface globalNavMeshSurface;

    private DoorDatabase db;

    private void Awake() {
        db = Resources.Load<DoorDatabase>("DoorDatabase");
    }

    private void Start()
    {
        GenerateNewLevel();
    }

    public void GenerateNewLevel()
    {
        Generate();
        StartCoroutine(BuildNavMesh());
        foreach (RoomExit exit in openExits)
        {
            exit.CloseExit();
        }
    }

    private IEnumerator BuildNavMesh()
    {
        yield return new WaitForEndOfFrame();
        globalNavMeshSurface.BuildNavMesh();
    } 

    public void Generate()
    {
        Room startRoom = InstantiateRoom(StartRoom, Vector3.zero, Quaternion.identity);
        spawnedRooms.Add(startRoom);
        openExits.AddRange(startRoom.GetExits());
        spawnedRoomsCount += 1;

        while (spawnedRoomsCount < maxRooms && openExits.Count > 0 && currentAttempts < maxAttempts)
        {
            RoomExit exitToConnect = PickRandomExit();

            Room prefabRoomToSpawn = PickCompatibleRoom(exitToConnect);

            if (prefabRoomToSpawn == null)
            {
                openExits.Remove(exitToConnect);
                continue;
            }

            Room newRoom = SpawnRoomAtExit(prefabRoomToSpawn, exitToConnect);
            currentAttempts += 1;

            if (newRoom == null)
            {
                continue;
            }

            if (CheckOverlap(newRoom))
            {
                Destroy(newRoom.gameObject);
                continue;
            }

            spawnedRooms.Add(newRoom);
            spawnedRoomsCount += 1;

            foreach (RoomExit newExit in newRoom.GetExits())
            {
                openExits.Add(newExit);
            }

            openExits.Remove(exitToConnect);
        }
    }

    private Room InstantiateRoom(Room prefab, Vector3 position, Quaternion rotation)
    {
        Room instance = Instantiate(prefab, position, rotation, transform);
        instance.name = prefab.name + "_Clone";
        instance.generationOrder = spawnedRoomsCount + 1;
        return instance;
    }

    private RoomExit PickRandomExit()
    {
        int index = Random.Range(0, openExits.Count);
        return openExits[index];
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

        if (compatibleRooms.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, compatibleRooms.Count);
        return compatibleRooms[randomIndex];
    }

    private Room SpawnRoomAtExit(Room prefab, RoomExit exitToConnect)
    {
        Quaternion rotation = Quaternion.LookRotation(exitToConnect.transform.forward, Vector3.up);
        Vector3 spawnPosition = exitToConnect.transform.position;
        Room newRoom = InstantiateRoom(prefab, spawnPosition, rotation);
        return newRoom;
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
                if (otherRoom != null && otherRoom != newRoom)
                {
                    if (newRoom.generationOrder > otherRoom.generationOrder)
                        return true;
                }
            }
        }

        return false;
    }
}