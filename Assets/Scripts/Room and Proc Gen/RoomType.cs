using UnityEngine;
using System.Collections.Generic;

public enum RoomType
{
    Start,
    Basic,
    Hallway,
    Large,
    Connector, 
}

public static class RoomTypeExtensions
{
    public static readonly Dictionary<RoomType, string> displayNames = new Dictionary<RoomType, string>
    {
        { RoomType.Start, "Starting Room" },
        { RoomType.Basic, "Basic Room" },
        { RoomType.Hallway, "Hallway" },
        { RoomType.Large, "Large Room" },
        { RoomType.Connector, "Connector Room" },
    };

    public static string ToDisplayName(this RoomType type)
    {
        return displayNames.TryGetValue(type, out var name) ? name : type.ToString();
    }
}