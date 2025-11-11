using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomType roomType;

    private BoxCollider[] roomBounds;

    public List<RoomExit> exits = new();

    public int generationOrder;

    public bool markForDelete = false;

    private void Awake()
    {
        if (exits == null || exits.Count == 0)
        {
            exits = GetExits();
        }

        if(roomBounds == null)
        {
            roomBounds = GetComponents<BoxCollider>();
        }
    }

    public void InitializeExits()
    {
        exits = new List<RoomExit>(GetComponentsInChildren<RoomExit>(true));
        foreach (var exit in exits)
            exit.parentRoom = this;
    }

    public List<RoomExit> GetExits()
    {
        if (exits == null || exits.Count == 0)
            InitializeExits();

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
            Debug.Log($"Deleting {name} (newer room overlapped with {otherRoom.name})");
        }
    }
}
