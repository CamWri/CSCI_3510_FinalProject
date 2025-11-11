using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomType roomType;

    private BoxCollider[] roomBounds;

    public List<RoomExit> exits = new();

    public int generationOrder;

    public bool markForDelete = false;

    public List<RoomExit> GetExits()
    {
        return exits;
    }

    private void OnTriggerEnter(Collider other)
    {
        Room otherRoom = other.GetComponentInParent<Room>();
        if (otherRoom == null) return;

        // Only handle once per pair
        if (generationOrder > otherRoom.generationOrder || generationOrder == 0)
        {
            markForDelete = true;
        }
    }
}
