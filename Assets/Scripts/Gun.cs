using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Weapon Identity")]
    public WeaponType weaponType;
    public WeaponRarity rarity;

    [Header("Stats Database")]
    public WeaponStatsDatabase database;

    [Header("Gun Data")]
    public bool isAuto = false;
    private float range;
    private float fireRate;
    private float damage;
    private int ammo;
    private float reloadTime;

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
        ApplyStats();

        fpsCamera = GameObject.Find("CameraHolder/Main Camera").GetComponent<Camera>();
        nextTimeToFire = 0.0f;
        currentAmmo = ammo;
        reloadMovementTime = reloadTime/4f;
        audioSource = GetComponent<AudioSource>();
        reloading = false;
        HUDController.Instance.UpdateWeaponText(currentAmmo);
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
        HUDController.Instance.UpdateWeaponText(currentAmmo);
        RaycastHit hit;

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            // Try to damage skeleton
            Skeleton skeleton = hit.transform.GetComponent<Skeleton>();
            if (skeleton != null)
            {
                Debug.Log("Shot at a skeleton and dealt" + damage.ToString() + "damage.");
                skeleton.TakeDamage(damage);
                PlayerMoneyManager.Instance.AddMoney(10);

                if (HUDController.Instance != null)
                    HUDController.Instance.ShowHitMarker();
            }
        }

        shootAnimation = true;
        shootAnimationStartTime = Time.time;
        nextTimeToFire = Time.time + fireRate;
    }

    void Reload()
    {
        currentAmmo = ammo;

        HUDController.Instance.UpdateWeaponText(currentAmmo);

        reloading = true;
        reloadStartTime = Time.time;
        audioSource.PlayOneShot(reloadSound, 0.7f);

        nextTimeToFire = Time.time + reloadTime;
    }

    public void ApplyStats()
    {
        damage = database.GetDamage(weaponType, rarity);
        ammo = database.GetAmmo(weaponType, rarity);
        fireRate = database.GetFireRate(weaponType, rarity);
        range = database.GetRange(weaponType, rarity);
        reloadTime = database.GetReloadTime(weaponType, rarity);
    }


    public void UpgradeWeapon()
    {
        if (rarity == WeaponRarity.Legendary)
            return;

        rarity++;
        ApplyStats();
        HUDController.Instance.UpdateWeaponText(currentAmmo);
    }
}
