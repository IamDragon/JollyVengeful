using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : Weapon
{
    public bool isFiring;
    protected bool isReloading;
    public bool IsReloading { get { return isReloading; } }


    [SerializeField] protected BulletController bulletType;
    [SerializeField] protected int bulletDamage;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected int magSize;
    [SerializeField] protected int currentMag;

    [SerializeField] protected float timeBetweenShots;
    protected bool canShoot;

    [SerializeField] protected float reloadTime;
    [SerializeField] protected float muzzleFlashTime;
    [SerializeField] protected float spreadFactor;
    [SerializeField] protected float bulletTTL;

    public Transform firePoint;
    [SerializeField] protected Transform muzzleFlash;

    [SerializeField] protected ParticleSystem rifleSmoke;

    public int CurrentMag => currentMag;
    public int MagSize => magSize;

    protected override void Start()
    {
        base.Start();
        if (!transform.parent)
            currentMag = magSize;
        else
        {
            StartReloading();
        }

    }
    protected override void Update()
    {
        base.Update();
        if (!isGrounded)
        {
            WhileHeld();

        }
    }

    protected virtual void WhileHeld()
    {
        Invoke(nameof(StopMuzzleFlash), muzzleFlashTime);

        if (isFiring && !isReloading)
        {
            if (CheckMagEmpty())
            {
                StartReloading();
            }
            if (canShoot)
            {
                Shoot();
                Invoke(nameof(ResetShot), timeBetweenShots);
            }
        }
    }

    //resets currentmag and reloadTimer, sets isReloading to false
    protected virtual void Reload()
    {
        isReloading = false;
        canShoot = true;
        StopReloadSound();
        if (transform.root.CompareTag("Player"))
        {
            PlayerActions.OnReloadEnd?.Invoke();
            currentMag = transform.root.GetComponent<Inventory>().GetReloadAmount(magSize, currentMag);
        }
        else
            currentMag = magSize;
        CancelInvoke();
    }

    public virtual void StartReloading()
    {
        ReloadSound();
        isReloading = true;
        canShoot = false;
        Invoke(nameof(Reload), reloadTime);

        if (transform.root.CompareTag("Player"))
            PlayerActions.OnReloadStart?.Invoke();
    }

    //spawns bullet, resets shotCounter, decreases currentMag
    protected virtual void Shoot()
    {
        SpawnBullet();
        ShootingSound();
        canShoot = false;
        currentMag--;
        PlayFX();
    }

    protected virtual void ShootingSound()
    {
        audioManager.Play("ShootingSound");
    }

    protected virtual void ReloadSound()
    {
        if (audioManager == null)
            return;
        if (!isReloading && !audioManager.IsSoundPlaying("ReloadSound"))
        {
            audioManager.Play("ReloadSound");
        }
    }

    protected virtual void StopReloadSound()
    {
        if (audioManager == null)
            return;
        if (audioManager.IsSoundPlaying("ReloadSound"))
        {
            audioManager.Stop("ReloadSound");
        }
    }

    protected virtual void ResetShot()
    {
        canShoot = true;
    }

    //returns true if currentMag <= 0
    protected bool CheckMagEmpty()
    {
        return currentMag <= 0;
    }

    //instansiates new Bullet with bullet variables
    protected void SpawnBullet()
    {
        BulletController newBullet = Instantiate(bulletType, firePoint.position, firePoint.rotation) as BulletController;

        newBullet.speed = bulletSpeed;
        newBullet.damageAmount = bulletDamage;
        newBullet.ttl = bulletTTL;
        //set bullets tag to guns tag, used to tell what should be damaged by bullet
        newBullet.tag = transform.parent.tag + "Bullet";
        newBullet.transform.Rotate(0, GetBulletRotation(), 0);
    }

    //returns random float between -spreadFactor and spreadFactor
    private float GetBulletRotation()
    {
        return Random.Range(-spreadFactor, spreadFactor);

    }

    public override void DropWeapon()
    {
        base.DropWeapon();
        isFiring = false;
    }

    protected override void PlayVFX()
    {
        muzzleFlash.gameObject.SetActive(true);
        if (!rifleSmoke.isPlaying)
        {
            rifleSmoke.Play();
        }
    }
    protected override void PlaySFX()
    {
        //Playgunshot sound
    }

    protected void StopMuzzleFlash()
    {
        muzzleFlash.gameObject.SetActive(false);
    }

    public override void EquipWeapon(Transform parent, string newTag, Quaternion rotation)
    {
        base.EquipWeapon(parent, newTag, rotation);
        if (currentMag <= 0)
            StartReloading();
        else
            canShoot = true;
    }
}