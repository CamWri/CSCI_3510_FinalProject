using UnityEngine;

public class WallBuy : MonoBehaviour
{
    public int weaponType;   // index for GunSwap
    public int weaponCost;   // cost of this weapon

    public void SwitchToPurchasedWeapon()
    {
        // Check if player has enough money
        if (!PlayerMoneyManager.Instance.SpendMoney(weaponCost))
        {
            Debug.Log("Cannot afford weapon! Cost: " + weaponCost + ", You have: " + PlayerMoneyManager.Instance.moneyCount);
            return;
        }

        // Find the player object (assumes it has the "Player" tag)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found!");
            return;
        }

        // Get the GunSwap component
        GunSwap gunSwap = player.GetComponent<GunSwap>();
        if (gunSwap == null)
        {
            Debug.LogError("GunSwap component not found on player!");
            return;
        }

        // Call SelectGun with the weaponType
        gunSwap.SelectGun(weaponType);
        Debug.Log("Weapon purchased! Weapon index: " + weaponType + ", Remaining money: " + PlayerMoneyManager.Instance.moneyCount);
    }
}