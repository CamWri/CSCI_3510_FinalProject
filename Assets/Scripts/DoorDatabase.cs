using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DoorDatabase", menuName = "Dungeon/Door Database")]
public class DoorDatabase : ScriptableObject
{
    [System.Serializable]
    public struct DoorEntry
    {
        public RoomExitType type;
        public GameObject prefab;
    }

    public List<DoorEntry> doors;

    private Dictionary<RoomExitType, GameObject> lookup;

    public GameObject GetDoorPrefab(RoomExitType type)
    {
        if (lookup == null)
        {
            lookup = new Dictionary<RoomExitType, GameObject>();
            foreach (var entry in doors)
                lookup[entry.type] = entry.prefab;
        }

        return lookup[type];
    }
}