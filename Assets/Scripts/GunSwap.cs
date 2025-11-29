using UnityEngine;

public class GunSwap : MonoBehaviour
{
    public GameObject[] guns;
    private int currentGunIndex = 0;
    private void Awake()
    {
        foreach (GameObject gun in guns) { 
            gun.SetActive(true);
            gun.SetActive(false);
        }
    }
    void Start()
    {
        Debug.Log("Current gun index: " + currentGunIndex);
        SelectGun(currentGunIndex);
    }
    public void SelectGun(int index)
    {

        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(i == index);
        }



        currentGunIndex = index;
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
