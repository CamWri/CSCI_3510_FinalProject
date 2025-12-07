using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomType roomType;

    private BoxCollider[] roomBounds;

    public List<RoomExit> exits = new();

    public List<EnemySpawnPoints> enemySpawnPoints = new();

    public int generationOrder;

    public bool markForDelete = false;

    public List<Transform> wallBuyLocations;

    public List<EnemySpawnPoints> GetEnemySpawnPoints()
    {
        return enemySpawnPoints;
    }

    public List<RoomExit> GetExits()
    {
        return exits;
    }

    public List<Transform> GetWallBuyLocations()
    {
        return wallBuyLocations;
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
