using UnityEngine;


public class RoomExit : MonoBehaviour
{
    public RoomType[] roomType;

    public Room parentRoom;

    public GameObject doorInstance;

    private int blockingObjects = 0;
    
    private void Awake()
    {
        doorInstance.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Room hitRoom = other.GetComponentInParent<Room>();

        if (hitRoom != null && hitRoom != parentRoom)
        {
            blockingObjects++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Room hitRoom = other.GetComponentInParent<Room>();

        if (hitRoom != null && hitRoom != parentRoom)
        {
            blockingObjects--;
        }

        if (blockingObjects <= 0)
        {
            blockingObjects = 0;
            CloseExit();
        }
    }

    public void CloseExit()
    {
        if (blockingObjects == 0)
        {
            doorInstance.SetActive(true);
        }
        else
        {
            Debug.Log($"{name}: CloseExit — blocked.");
        }
    }
}