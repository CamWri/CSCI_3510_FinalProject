using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Identity")]
    public WeaponType weaponType;
    public WeaponRarity currentRarity = WeaponRarity.Base;

    [Header("Runtime Stats (auto-filled from database)")]
    public int currentAmmo;
    public float currentDamage;

    [Header("Config")]
    public WeaponStatsDatabase statsDatabase;

    private void Start()
    {
        ApplyStatsFromDatabase();
    }

    /// <summary>
    /// Pulls ammo and damage for this weapon's type + rarity.
    /// Call this after changing rarity (e.g., upgrades).
    /// </summary>
    public void ApplyStatsFromDatabase()
    {
        if (statsDatabase == null)
        {
            Debug.LogError($"[{name}] Weapon has no WeaponStatsDatabase assigned!");
            return;
        }

        (currentAmmo, currentDamage) = statsDatabase.GetStats(weaponType, currentRarity);
    }

    /// <summary>
    /// Upgrade rarity by one step if not already Legendary.
    /// </summary>
    public void UpgradeRarity()
    {
        if (currentRarity == WeaponRarity.Legendary)
            return;

        currentRarity++;
        ApplyStatsFromDatabase();
    }

    /// <summary>
    /// Refills ammo to the max for the current rarity.
    /// </summary>
    public void RefillAmmoToMax()
    {
        if (statsDatabase == null) return;

        (int maxAmmo, _) = statsDatabase.GetStats(weaponType, currentRarity);
        currentAmmo = maxAmmo;
    }
}
