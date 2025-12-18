using UnityEngine;

[System.Serializable]
public class WeaponStatBlock
{
    public WeaponType weaponType;

    [Header("Base (Starter)")]
    public float baseDamage = 10;
    public int baseAmmo = 10;
    public float baseFireRate = 10;
    public float baseRange = 50;
    public float baseReloadTime = 1f;

    [Header("Common")]
    public float commonDamage = 15;
    public int commonAmmo = 12;
    public float commonFireRate = 10;
    public float commonRange = 50;
    public float commonReloadTime = 1f;

    [Header("Rare")]
    public float rareDamage = 20;
    public int rareAmmo = 14;
    public float rareFireRate = 10;
    public float rareRange = 50;
    public float rareReloadTime = 1f;

    [Header("Epic")]
    public float epicDamage = 25;
    public int epicAmmo = 16;
    public float epicFireRate = 10;
    public float epicRange = 50;
    public float epicReloadTime = 1f;

    [Header("Legendary")]
    public float legendaryDamage = 30;
    public int legendaryAmmo = 18;
    public float legendaryFireRate = 10;
    public float legendaryRange = 50;
    public float legendaryReloadTime = 1f;
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

    public float GetDamage(WeaponType type, WeaponRarity rarity)
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


    public float GetFireRate(WeaponType type, WeaponRarity rarity)
    {
        var stats = GetStatsFor(type);
        if (stats == null) return 0;

        switch (rarity)
        {
            case WeaponRarity.Base: return stats.baseFireRate;
            case WeaponRarity.Common: return stats.commonFireRate;
            case WeaponRarity.Rare: return stats.rareFireRate;
            case WeaponRarity.Epic: return stats.epicFireRate;
            case WeaponRarity.Legendary: return stats.legendaryFireRate;
            default: return stats.baseFireRate;
        }
    }

    public float GetRange(WeaponType type, WeaponRarity rarity)
    {
        var stats = GetStatsFor(type);
        if (stats == null) return 0;

        switch (rarity)
        {
            case WeaponRarity.Base: return stats.baseRange;
            case WeaponRarity.Common: return stats.commonRange;
            case WeaponRarity.Rare: return stats.rareRange;
            case WeaponRarity.Epic: return stats.epicRange;
            case WeaponRarity.Legendary: return stats.legendaryRange;
            default: return stats.baseRange;
        }
    }

    public float GetReloadTime(WeaponType type, WeaponRarity rarity)
    {
        var stats = GetStatsFor(type);
        if (stats == null) return 0;

        switch (rarity)
        {
            case WeaponRarity.Base: return stats.baseReloadTime;
            case WeaponRarity.Common: return stats.commonReloadTime;
            case WeaponRarity.Rare: return stats.rareReloadTime;
            case WeaponRarity.Epic: return stats.epicReloadTime;
            case WeaponRarity.Legendary: return stats.legendaryReloadTime;
            default: return stats.baseAmmo;
        }
    }
}
