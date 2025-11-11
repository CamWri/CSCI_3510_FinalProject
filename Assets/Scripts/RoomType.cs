using UnityEngine;
using System.Collections.Generic;

public enum RoomType
{
    Start,
    Basic,
    Hallway,
    LShape, 
}

public static class RoomTypeExtensions
{
    public static readonly Dictionary<RoomType, string> displayNames = new Dictionary<RoomType, string>
    {
        { RoomType.Start, "Starting Room" },
        { RoomType.Basic, "Basic Room" },
        { RoomType.Hallway, "Hallway" },
        { RoomType.LShape, "L Shape Room" },
    };

    public static string ToDisplayName(this RoomType type)
    {
        return displayNames.TryGetValue(type, out var name) ? name : type.ToString();
    }
}