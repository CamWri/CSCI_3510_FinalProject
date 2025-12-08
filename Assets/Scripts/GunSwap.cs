using UnityEngine;

public class GunSwap : MonoBehaviour
{
    public GameObject[] guns;  // must match enum order
    public WeaponType currentGunType = WeaponType.Pistol;
    private GameObject currentGun;      // store the actual gun instance
    private Gun currentGunScript;       // optional: store the gun logic component

    private void Awake()
    {
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

        currentGunScript = currentGun.GetComponent<Gun>(); // optional
        Debug.Log("Equipped gun: " + type);
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