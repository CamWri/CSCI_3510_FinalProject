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
        Gun currentGun = gunSwap?.GetCurrentGunScript();

        WeaponRarity nextRarity = GetNextRarity(currentGun, weaponType);

        if (currentGun == null || currentGun.weaponType != weaponType)
        {
            // Different weapon → buy
            interactable.message = $"Buy {weaponType} - ${baseWeaponCost}";
        }
        else if (currentGun.rarity == WeaponRarity.Legendary)
        {
            // Max rarity → maybe show “MAXED”
            interactable.message = $"{weaponType} is MAXED!";
        }
        else
        {
            // Upgrade message showing next rarity
            int upgradeCost = GetUpgradeCost(currentGun.rarity);
            interactable.message = $"Upgrade {weaponType}\n{currentGun.rarity} → {nextRarity}\n${upgradeCost}";
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

    WeaponRarity GetNextRarity(Gun currentGun, WeaponType wallWeaponType)
    {
        // Case 1 – different weapon → first purchase
        if (currentGun == null || currentGun.weaponType != wallWeaponType)
            return WeaponRarity.Base;

        // Case 2 – same weapon → next rarity
        if (currentGun.rarity == WeaponRarity.Legendary)
            return WeaponRarity.Legendary; // maxed out

        return currentGun.rarity + 1;
    }
}