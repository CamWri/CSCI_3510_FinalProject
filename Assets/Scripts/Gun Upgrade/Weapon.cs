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

    public void ApplyStatsFromDatabase()
    {
        if (statsDatabase == null)
        {
            Debug.LogError($"[{name}] Weapon has no WeaponStatsDatabase assigned!");
            return;
        }

        (currentAmmo, currentDamage) = statsDatabase.GetStats(weaponType, currentRarity);
    }

    public bool CanUpgrade()
    {
        return currentRarity < WeaponRarity.Legendary;
    }

    public void UpgradeRarity()
    {
        if (!CanUpgrade()) return;

        currentRarity++;
        ApplyStatsFromDatabase();
    }

    public void RefillAmmoToMax()
    {
        if (statsDatabase == null) return;

        (int maxAmmo, _) = statsDatabase.GetStats(weaponType, currentRarity);
        currentAmmo = maxAmmo;
    }
}
