using UnityEngine;


public class RoomExit : MonoBehaviour
{
    public RoomExitType exitType = RoomExitType.Door;

    public RoomType[] roomType;

    public Room parentRoom;

    public bool hasSpawned = false;
}
