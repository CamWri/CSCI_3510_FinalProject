using UnityEngine;

public class GunSwap : MonoBehaviour
{
    public GameObject[] guns;
    private int currentGunIndex = 0;

    private void Awake()
    {
        // Enable only the first gun
        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i] != null)
                guns[i].SetActive(i == currentGunIndex);
        }
    }

    public void SelectGun(int index)
    {
        if (index < 0 || index >= guns.Length) return;

        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i] != null)
                guns[i].SetActive(i == index);
        }

        currentGunIndex = index;
        Debug.Log("Gun selected: " + index);
    }

    // -----------------------------
    // NEW HELPERS FOR WALL BUY
    // -----------------------------

    /// <summary>
    /// Returns the gun GameObject stored at the given index.
    /// </summary>
    public GameObject GetGunByIndex(int index)
    {
        if (guns == null || guns.Length == 0) return null;
        if (index < 0 || index >= guns.Length) return null;

        return guns[index];
    }

    /// <summary>
    /// Returns the Weapon component on the gun at the index (if present).
    /// </summary>
    public Weapon GetWeaponComponentByIndex(int index)
    {
        GameObject gun = GetGunByIndex(index);
        if (gun == null) return null;

        return gun.GetComponent<Weapon>();
    }

    /// <summary>
    /// Useful if you ever want "upgrade current gun" behavior.
    /// </summary>
    public int GetCurrentGunIndex()
    {
        return currentGunIndex;
    }

    public GameObject GetCurrentGun()
    {
        return GetGunByIndex(currentGunIndex);
    }

    public Weapon GetCurrentWeaponComponent()
    {
        return GetWeaponComponentByIndex(currentGunIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectGun(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectGun(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectGun(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectGun(3);
        }
    }
}
