using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float range = 100f;
    public float fireRate = 1.0f;
    public float damage = 30f;
    public float ammo = 30f;

    public ParticleSystem muzzleFlash;

    private Camera fpsCamera;
    private float nextTimeToFire;
    private void Start()
    {
        fpsCamera = GameObject.Find("CameraHolder/Main Camera").GetComponent<Camera>();
        nextTimeToFire = 0.0f;
    }
    private void Update()
    {
        bool ready = Time.time > nextTimeToFire;

        if (Input.GetButtonDown("Fire1") && ready)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

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
}
