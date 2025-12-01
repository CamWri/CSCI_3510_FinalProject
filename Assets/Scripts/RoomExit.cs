using UnityEngine;


public class RoomExit : MonoBehaviour
{
    public RoomType[] roomType;

    [Header("References")]
    public Room parentRoom;
    public GameObject doorPrefab;    // the doorway prefab
    public GameObject wallPrefab;    // the wall prefab

    private int blockingObjects = 0;
    
    public bool collisionIsEntryWay = false;

    private void Awake()
    {
        // Optional: deactivate both, we'll spawn later
        if (doorPrefab != null) doorPrefab.SetActive(false);
        if (wallPrefab != null) wallPrefab.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has a Room component (normal blocking check)
        Room hitRoom = other.GetComponentInParent<Room>();
        if (hitRoom != null && hitRoom != parentRoom)
        {
            blockingObjects++;
        }

        // Check if the other collider is tagged "Exit"
        if (other.CompareTag("Exit"))
        {
            collisionIsEntryWay = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Room hitRoom = other.GetComponentInParent<Room>();
        if (hitRoom != null && hitRoom != parentRoom)
        {
            blockingObjects--;
            if (blockingObjects < 0) blockingObjects = 0;
        }

        if (other.CompareTag("Exit"))
        {
            collisionIsEntryWay = false;
        }
    }


    private void SpawnDoorway()
    {
        if (doorPrefab != null)
            doorPrefab.SetActive(true);
        if (wallPrefab != null)
            wallPrefab.SetActive(false);
    }

    private void SpawnWall()
    {
        if (wallPrefab != null)
            wallPrefab.SetActive(true);
        if (doorPrefab != null)
            doorPrefab.SetActive(false);
    }

    public void CloseExit()
    {
        // Case 1: This exit was used to spawn another room
        if (collisionIsEntryWay)
        {
            SpawnDoorway();
        }
        // Case 2: Exit collides with another exit
        else if (blockingObjects > 0)
        {
            SpawnWall();
        }
        // Case 3: Otherwise, spawn wall
        else
        {
            SpawnWall();
        }
    }
}