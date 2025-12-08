using UnityEngine;

[System.Serializable]
public class WeaponTypeStats
{
    [Header("Which Weapon Type?")]
    public WeaponType weaponType;

    [Header("Base")]
    public int baseAmmo = 60;
    public float baseDamage = 10f;

    [Header("Common")]
    public int commonAmmo = 70;
    public float commonDamage = 12f;

    [Header("Rare")]
    public int rareAmmo = 80;
    public float rareDamage = 14f;

    [Header("Epic")]
    public int epicAmmo = 90;
    public float epicDamage = 17f;

    [Header("Legendary")]
    public int legendaryAmmo = 100;
    public float legendaryDamage = 20f;
}

[CreateAssetMenu(fileName = "WeaponStatsDatabase", menuName = "Weapons/Weapon Stats Database")]
public class WeaponStatsDatabase : ScriptableObject
{
    [Tooltip("One entry per weapon type (Pistol, SMG, AR, MG).")]
    public WeaponTypeStats[] weaponTypeStats;

    public WeaponTypeStats GetStatsForType(WeaponType type)
    {
        if (weaponTypeStats == null) return null;

        foreach (var s in weaponTypeStats)
        {
            if (s != null && s.weaponType == type)
                return s;
        }

        Debug.LogWarning($"[WeaponStatsDatabase] No stats found for weapon type: {type}");
        return null;
    }

    public (int ammo, float damage) GetStats(WeaponType type, WeaponRarity rarity)
    {
        var s = GetStatsForType(type);
        if (s == null) return (0, 0f);

        switch (rarity)
        {
            case WeaponRarity.Base:
                return (s.baseAmmo, s.baseDamage);
            case WeaponRarity.Common:
                return (s.commonAmmo, s.commonDamage);
            case WeaponRarity.Rare:
                return (s.rareAmmo, s.rareDamage);
            case WeaponRarity.Epic:
                return (s.epicAmmo, s.epicDamage);
            case WeaponRarity.Legendary:
                return (s.legendaryAmmo, s.legendaryDamage);
            default:
                return (s.baseAmmo, s.baseDamage);
        }
    }
}
