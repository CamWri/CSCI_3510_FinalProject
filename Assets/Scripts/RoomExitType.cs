using UnityEngine;
using System.Collections.Generic;

public enum RoomExitType
{
    WoodenRoundDoor,
    WoodenSquareDoor,
}

public static class RoomExitTypeExtension
{
    public static readonly Dictionary<RoomExitType, string> displayNames = new Dictionary<RoomExitType, string>
    {
        { RoomExitType.WoodenRoundDoor, "Wooden Round Door" },
        { RoomExitType.WoodenSquareDoor, "Wooden Square Door" },
    };

    public static string ToDisplayName(this RoomExitType type)
    {
        return displayNames.TryGetValue(type, out var name) ? name : type.ToString();
    }
}