using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstGun : GunController
{
    public int shotsPerBurst;
    public float timeBetweenBursts;

    private int burstCounter;
    private bool isBursting;
    private bool canBurst;

    protected override void Start()
    {
        base.Start();
        if (CheckMagEmpty())
            Reload();
        else
            canBurst = true;
    }

    protected override void WhileHeld()
    {

        Invoke(nameof(StopMuzzleFlash), muzzleFlashTime);

        if (isBursting && !isReloading)
        {
            if (CheckMagEmpty())
            {
                StartReloading();
            }
            else if (BurstFinished())
            {
                OnBurstFinish();
            }
            else if(canShoot)
            {
                Shoot();
                Invoke(nameof(ResetShot), timeBetweenShots);

            }
        }
        else if (isFiring && !isReloading && !isBursting)
        {
            if (CheckMagEmpty())
            {
                StartReloading();
            }
            if (canBurst)
            {
                StartBurst();
            }
        }
    }

    protected override void Shoot()
    {
        SpawnBullet();
        ShootingSound();
        burstCounter--;
        currentMag--;
        PlayFX();
        canShoot = false;
        
    }

    public override void StartReloading()
    {
        base.StartReloading();
        isBursting = false;
        canBurst = false;
    }


    protected override void Reload()
    {
        base.Reload();
        canBurst = true;
        burstCounter = shotsPerBurst;
    }

    private void StartBurst()
    {
        isBursting = true;
        canBurst = false;
        canShoot = true;
        Invoke(nameof(ResetBurst), timeBetweenBursts);
    }

    private void ResetBurst()
    {
        canBurst = true;
    }

    private bool BurstFinished()
    {
        return burstCounter <= 0;
    }

    private void OnBurstFinish()
    {
        isBursting = false;
        burstCounter = shotsPerBurst;
    }

    protected override void ResetShot()
    {
        canShoot = true;
    }
}
