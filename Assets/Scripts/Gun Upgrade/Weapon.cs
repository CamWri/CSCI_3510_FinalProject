using UnityEngine;

public class Weapon : MonoBehaviour
{
    /*[Header("Setup")]
    public WeaponType weaponType;
    public WeaponRarity currentRarity = WeaponRarity.Base;
    public WeaponStatsDatabase statsDatabase;

    [Header("Runtime Stats (read-only)")]
    [SerializeField] private int currentDamage;
    [SerializeField] private int currentMagSize;
    [SerializeField] private int currentAmmoInMag;

    public int CurrentDamage => currentDamage;
    public int CurrentMagSize => currentMagSize;
    public int CurrentAmmoInMag => currentAmmoInMag;

    private void Awake()
    {
        ApplyStats();
    }

    /// <summary>
    /// Pulls damage & ammo from the WeaponStatsDatabase based on weapon type + rarity.
    /// </summary>
    public void ApplyStats()
    {
        if (statsDatabase == null)
        {
            Debug.LogError($"Weapon {name} has no WeaponStatsDatabase assigned!");
            return;
        }

        currentDamage = statsDatabase.GetDamage(weaponType, currentRarity);
        currentMagSize = statsDatabase.GetAmmo(weaponType, currentRarity);
        currentAmmoInMag = currentMagSize; // full mag on change
    }

    /// <summary>
    /// Move up exactly one rarity tier (up to Legendary).
    /// </summary>
    public void UpgradeRarity()
    {
        if (currentRarity == WeaponRarity.Legendary)
            return;

        currentRarity++;
        ApplyStats();
    }

    /// <summary>
    /// Refills the mag to max.
    /// </summary>
    public void RefillAmmo()
    {
        currentAmmoInMag = currentMagSize;
    }

    /// <summary>
    /// Try to spend ammo from mag (returns false if not enough).
    /// Hook this into your shooting script.
    /// </summary>
    public bool ConsumeAmmo(int amount)
    {
        if (currentAmmoInMag < amount)
            return false;

        currentAmmoInMag -= amount;
        return true;
    }*/
}
