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
