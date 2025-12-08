using UnityEngine;

public class WallBuy : MonoBehaviour
{
    public WeaponType weaponType;
    public int baseWeaponCost = 1500;

    public Interactable interactable; // assign in inspector

    private void Start()
    {
        UpdateMessage();
    }

    private void UpdateMessage()
    {
        GunSwap gunSwap = GameObject.FindGameObjectWithTag("Player")?.GetComponent<GunSwap>();

        if (gunSwap == null || gunSwap.GetCurrentGunScript() == null)
        {
            interactable.message = $"Buy {weaponType} - ${baseWeaponCost}";
            return;
        }

        Gun gun = gunSwap.GetCurrentGunScript();

        // If same weapon → show upgrade price
        if (gun.weaponType == weaponType)
        {
            int upgradeCost = GetUpgradeCost(gun.rarity);
            interactable.message = $"Upgrade {weaponType} ({gun.rarity}) → ${upgradeCost}";
        }
        else
        {
            // Different weapon → show buy price
            interactable.message = $"Buy {weaponType} - ${baseWeaponCost}";
        }
    }

    public void SwitchOrUpgradeWeapon()
    {
        GunSwap gunSwap = GameObject.FindGameObjectWithTag("Player")?.GetComponent<GunSwap>();
        if (gunSwap == null) return;

        Gun current = gunSwap.GetCurrentGunScript();

        // Case 1 – Same weapon → Upgrade
        if (current != null && current.weaponType == weaponType)
        {
            int cost = GetUpgradeCost(current.rarity);

            if (!PlayerMoneyManager.Instance.SpendMoney(cost))
            {
                Debug.Log("Not enough money to upgrade!");
                return;
            }

            current.UpgradeWeapon();
            UpdateMessage();
            return;
        }

        // Case 2 – Different weapon → Buy & equip
        if (!PlayerMoneyManager.Instance.SpendMoney(baseWeaponCost))
        {
            Debug.Log("Not enough money to buy weapon!");
            return;
        }

        gunSwap.SelectGun(weaponType);
        UpdateMessage();
    }

    public int GetUpgradeCost(WeaponRarity rarity)
    {
        switch (rarity)
        {
            case WeaponRarity.Base: return baseWeaponCost * 5;
            case WeaponRarity.Common: return baseWeaponCost * 10;
            case WeaponRarity.Rare: return baseWeaponCost * 20;
            case WeaponRarity.Epic: return baseWeaponCost * 40;
            default: return 999999;
        }
    }
}