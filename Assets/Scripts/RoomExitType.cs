using UnityEngine;
using System.Collections.Generic;

public enum RoomExitType
{
    Door
}

public static class RoomExitTypeExtension
{
    public static readonly Dictionary<RoomExitType, string> displayNames = new Dictionary<RoomExitType, string>
    {
        { RoomExitType.Door, "Door Exit" },
    };

    public static string ToDisplayName(this RoomExitType type)
    {
        return displayNames.TryGetValue(type, out var name) ? name : type.ToString();
    }
}