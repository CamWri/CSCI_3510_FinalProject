using UnityEngine;

[System.Serializable]
public class WeaponStatBlock
{
    public WeaponType weaponType;

    [Header("Base (Starter)")]
    public int baseDamage = 10;
    public int baseAmmo = 10;

    [Header("Common")]
    public int commonDamage = 15;
    public int commonAmmo = 12;

    [Header("Rare")]
    public int rareDamage = 20;
    public int rareAmmo = 14;

    [Header("Epic")]
    public int epicDamage = 25;
    public int epicAmmo = 16;

    [Header("Legendary")]
    public int legendaryDamage = 30;
    public int legendaryAmmo = 18;
}

[CreateAssetMenu(fileName = "WeaponStatsDatabase", menuName = "Weapons/Weapon Stats Database")]
public class WeaponStatsDatabase : ScriptableObject
{
    public WeaponStatBlock[] weaponStats;

    public WeaponStatBlock GetStatsFor(WeaponType type)
    {
        foreach (var block in weaponStats)
        {
            if (block.weaponType == type)
                return block;
        }

        Debug.LogWarning($"WeaponStatsDatabase: No stats found for weapon type {type}");
        return null;
    }

    public int GetDamage(WeaponType type, WeaponRarity rarity)
    {
        var stats = GetStatsFor(type);
        if (stats == null) return 0;

        switch (rarity)
        {
            case WeaponRarity.Base: return stats.baseDamage;
            case WeaponRarity.Common: return stats.commonDamage;
            case WeaponRarity.Rare: return stats.rareDamage;
            case WeaponRarity.Epic: return stats.epicDamage;
            case WeaponRarity.Legendary: return stats.legendaryDamage;
            default: return stats.baseDamage;
        }
    }

    public int GetAmmo(WeaponType type, WeaponRarity rarity)
    {
        var stats = GetStatsFor(type);
        if (stats == null) return 0;

        switch (rarity)
        {
            case WeaponRarity.Base: return stats.baseAmmo;
            case WeaponRarity.Common: return stats.commonAmmo;
            case WeaponRarity.Rare: return stats.rareAmmo;
            case WeaponRarity.Epic: return stats.epicAmmo;
            case WeaponRarity.Legendary: return stats.legendaryAmmo;
            default: return stats.baseAmmo;
        }
    }
}
