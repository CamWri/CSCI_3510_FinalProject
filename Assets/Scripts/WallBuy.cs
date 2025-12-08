using UnityEngine;

public class WallBuy : MonoBehaviour
{
    [Header("Weapon Index For GunSwap")]
    public int weaponType;   // index for GunSwap

    [Header("Costs")]
    public int weaponCost;   // cost of this weapon
    public int ammoCost = 500;
    public int upgradeCost = 1000;

    // ----------------------------
    // AUTO HUD TEXT SETUP
    // ----------------------------
    private void Awake()
    {
        SyncInteractableText();
    }

    private void OnValidate()
    {
        SyncInteractableText();
    }

    private void SyncInteractableText()
    {
        var interactable = GetComponent<Interactable>();
        if (interactable == null) return;

        // Primary message
        interactable.message = $"Buy Weapon for {weaponCost}";

        // Enable + set secondary
        interactable.hasSecondaryInteraction = true;
        interactable.secondaryMessage = $"Upgrade for {upgradeCost}";
    }

    // ----------------------------
    // BUY WEAPON
    // ----------------------------
    public void SwitchToPurchasedWeapon()
    {
        if (!PlayerMoneyManager.Instance.SpendMoney(weaponCost))
        {
            Debug.Log("Cannot afford weapon! Cost: " + weaponCost + ", You have: " + PlayerMoneyManager.Instance.moneyCount);
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found!");
            return;
        }

        GunSwap gunSwap = player.GetComponent<GunSwap>();
        if (gunSwap == null)
        {
            Debug.LogError("GunSwap component not found on player!");
            return;
        }

        gunSwap.SelectGun(weaponType);
        Debug.Log("Weapon purchased! Weapon index: " + weaponType + ", Remaining money: " + PlayerMoneyManager.Instance.moneyCount);
    }

    // ----------------------------
    // UPGRADE WEAPON
    // ----------------------------
    public void UpgradePurchasedWeapon()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found!");
            return;
        }

        GunSwap gunSwap = player.GetComponent<GunSwap>();
        if (gunSwap == null)
        {
            Debug.LogError("GunSwap component not found on player!");
            return;
        }

        Weapon weapon = gunSwap.GetWeaponComponentByIndex(weaponType);
        if (weapon == null)
        {
            Debug.Log("No Weapon component found for this wall buy index. Make sure that gun prefab has Weapon.cs.");
            return;
        }

        if (!weapon.CanUpgrade())
        {
            Debug.Log("Weapon is already Legendary.");
            return;
        }

        if (!PlayerMoneyManager.Instance.SpendMoney(upgradeCost))
        {
            Debug.Log("Cannot afford upgrade! Cost: " + upgradeCost + ", You have: " + PlayerMoneyManager.Instance.moneyCount);
            return;
        }

        weapon.UpgradeRarity();
        Debug.Log($"Weapon upgraded! Index: {weaponType}, New Rarity: {weapon.currentRarity}, Remaining money: {PlayerMoneyManager.Instance.moneyCount}");
    }

    // ----------------------------
    // BUY AMMO (optional)
    // ----------------------------
    public void BuyAmmoForPurchasedWeapon()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found!");
            return;
        }

        GunSwap gunSwap = player.GetComponent<GunSwap>();
        if (gunSwap == null)
        {
            Debug.LogError("GunSwap component not found on player!");
            return;
        }

        Weapon weapon = gunSwap.GetWeaponComponentByIndex(weaponType);
        if (weapon == null)
        {
            Debug.Log("No Weapon component found for this wall buy index. Make sure that gun prefab has Weapon.cs.");
            return;
        }

        if (!PlayerMoneyManager.Instance.SpendMoney(ammoCost))
        {
            Debug.Log("Cannot afford ammo! Cost: " + ammoCost + ", You have: " + PlayerMoneyManager.Instance.moneyCount);
            return;
        }

        weapon.RefillAmmoToMax();
        Debug.Log($"Ammo refilled! Index: {weaponType}, Remaining money: {PlayerMoneyManager.Instance.moneyCount}");
    }
}
