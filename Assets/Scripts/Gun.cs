using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float range = 100f;
    public float fireRate = 1.0f;
    public float damage = 30f;
    public int ammo = 30;
    public float reloadTime;

    public ParticleSystem muzzleFlash;

    private Camera fpsCamera;
    private float nextTimeToFire;

    int currentAmmo;
    private void Start()
    {
        fpsCamera = GameObject.Find("CameraHolder/Main Camera").GetComponent<Camera>();
        nextTimeToFire = 0.0f;
        currentAmmo = ammo;
    }
    private void Update()
    {
        bool ready = Time.time > nextTimeToFire;

        if (Input.GetButtonDown("Fire1") && ready && currentAmmo != 0)
        {
            Shoot();
        }
        else if(currentAmmo == 0)
        {
            Reload();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        currentAmmo -= 1;
        Debug.Log("Current ammo: " + currentAmmo);
        RaycastHit hit;

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null) 
            {
                target.Process(hit, damage);
            }
        }

        nextTimeToFire = Time.time + fireRate;
    }

    void Reload()
    {
        currentAmmo = ammo;
        nextTimeToFire = Time.time + reloadTime;
        Debug.Log("Reloading....");
    }
}
