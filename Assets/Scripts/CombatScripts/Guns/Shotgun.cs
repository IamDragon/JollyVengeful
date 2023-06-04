using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunController
{
    public int bulletsPerShot;

    protected override void Shoot()
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            SpawnBullet();
        }
        ShootingSound();
        PlayFX();
        currentMag--;
        canShoot = false;
    }

    protected override void ShootingSound()
    {
        audioManager.Play("ShotgunShot");
    }




}
