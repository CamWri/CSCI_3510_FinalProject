using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float range = 100f;
    public float fireRate = 1.0f;
    public float damage = 30f;
    public int ammo = 30;
    public float reloadTime = 1f;
    float reloadMovementTime;

    Vector3 currentPosition;

    public ParticleSystem muzzleFlash;

    public AudioSource audioSource;
    public AudioClip fireSound;

    private Camera fpsCamera;
    private float nextTimeToFire;

    float reloadStartTime;
    bool reloading;
    int currentAmmo;
    private void Start()
    {
        fpsCamera = GameObject.Find("CameraHolder/Main Camera").GetComponent<Camera>();
        nextTimeToFire = 0.0f;
        currentAmmo = ammo;
        reloadMovementTime = reloadTime/4f;
        audioSource = GetComponent<AudioSource>();
        reloading = false;
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
            currentPosition = transform.localPosition;
        }
        if (Input.GetKeyDown(KeyCode.R) && ready)
        {
            Reload();
            currentPosition = transform.localPosition;
            
        }
        if (reloading)
        {
            transform.localPosition = Vector3.Lerp(currentPosition, new Vector3(transform.localPosition.x, transform.localPosition.y - .6f, transform.localPosition.z), -4 * Math.Abs(Time.time - reloadStartTime - 2 * reloadMovementTime) / reloadTime + 2);
            if (Time.time > nextTimeToFire)
            {
                currentAmmo = ammo;
                reloading = false;
                Debug.Log("Reload Done");
            }
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
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
        reloading = true;
        reloadStartTime = Time.time;
        //reload movement
        // Reload sound


        Debug.Log("Reloading....");
        nextTimeToFire = Time.time + reloadTime;
    }
}
