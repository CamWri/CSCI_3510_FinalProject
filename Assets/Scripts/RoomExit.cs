using UnityEngine;


public class RoomExit : MonoBehaviour
{
    public RoomExitType exitType = RoomExitType.Door;

    public RoomType roomType;

    public Room parentRoom;

    public Transform roomExit;

    public bool hasSpawned = false;

    private void Awake()
    {
        parentRoom = GetComponentInParent<Room>();
        roomExit = transform;
    }
}
