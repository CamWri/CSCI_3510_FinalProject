using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Data")]
    public float range = 100f;
    public float fireRate = 1.0f;
    public float damage = 30f;
    public int ammo = 30;
    public float reloadTime = 1f;
    public bool isAuto = false;

    private float nextTimeToFire;
    float reloadStartTime;
    bool reloading;
    int currentAmmo;
    [Header("Gun Animations/Sound")]
    public ParticleSystem muzzleFlash;
    public AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip reloadSound;

    float reloadMovementTime;
    bool shootAnimation;
    float shootAnimationStartTime;
    Vector3 currentPosition;
    [Header("Gun Recoil")]
    public float recoilAmount;
    public Vector2 maxRecoil;
    public float recoilSpeed;
    public float resetRecoilSpeed;

    [Header("Camera Input Object")]
    public PlayerCam playerCam;
    private Camera fpsCamera;

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

        if ((isAuto ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1")) && ready && currentAmmo != 0)
        {
            Shoot();
            currentPosition = transform.localPosition;
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
        if (shootAnimation)
        {
            transform.localPosition = Vector3.Lerp(currentPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - .01f), -Math.Abs(4 * (Time.time - shootAnimationStartTime) - fireRate) / fireRate + 1f);
            if(Time.time > (shootAnimationStartTime + fireRate/2))
            {
                shootAnimation = false;
            }
        }

        playerCam.ResetRecoil(resetRecoilSpeed);
    }

    void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Emit(1);
            muzzleFlash.Play();
        }
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound, 0.7f);
        }
        playerCam.ApplyRecoil(maxRecoil, recoilAmount, recoilSpeed);

        currentAmmo -= 1;
        Debug.Log("Current ammo: " + currentAmmo);
        RaycastHit hit;

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            Debug.Log("Ray hit: " + hit.collider.name + " (gameObject: " + hit.collider.gameObject.name + ")");
            // Try to damage skeleton
            Skeleton skeleton = hit.transform.GetComponent<Skeleton>();
            if (skeleton != null)
            {
                Debug.Log("Shot at a skeleton");
                skeleton.TakeDamage(damage);
                PlayerMoneyManager.Instance.AddMoney(10);
            }
        }

        shootAnimation = true;
        shootAnimationStartTime = Time.time;
        nextTimeToFire = Time.time + fireRate;
    }

    void Reload()
    {
        currentAmmo = ammo;
        reloading = true;
        reloadStartTime = Time.time;
        audioSource.PlayOneShot(reloadSound, 0.7f);

        Debug.Log("Reloading....");
        nextTimeToFire = Time.time + reloadTime;
    }
}
