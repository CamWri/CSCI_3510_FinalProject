using UnityEngine;

public class GunSwap : MonoBehaviour
{
    private static GunSwap Instance;
    public GameObject[] guns;  // must match enum order
    public WeaponType currentGunType = WeaponType.Pistol;
    private GameObject currentGun;      // store the actual gun instance
    private Gun currentGunScript;       // optional: store the gun logic component

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        EquipGun(currentGunType);
    }

    public void SelectGun(WeaponType type)
    {
        EquipGun(type);
    }

    private void EquipGun(WeaponType type)
    {
        int index = (int)type;

        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i] != null)
                guns[i].SetActive(i == index);
        }

        currentGunType = type;
        currentGun = guns[index];

        if (currentGun == null)
        {
            Debug.LogError("GunSwap: Selected gun GameObject is null at index " + index);
            return;
        }

        currentGunScript = currentGun.GetComponent<Gun>();

        if (currentGunScript == null)
        {
            Debug.LogError("GunSwap: No Gun component found on " + currentGun.name);
            return;
        }

        if (HUDController.Instance != null)
        {
            int ammo = currentGunScript.database.GetAmmo(currentGunType, currentGunScript.rarity);

            HUDController.Instance.UpdateWeaponText(ammo, ammo);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectGun(WeaponType.Pistol);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectGun(WeaponType.AR);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectGun(WeaponType.SMG);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectGun(WeaponType.Shotgun);
    }

    public Gun GetCurrentGunScript()
    {
        return currentGunScript;
    }
}