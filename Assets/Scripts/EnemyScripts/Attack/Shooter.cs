using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{
    GunController gun;
    protected override void Start()
    {
        base.Start();
        gun = activeWeapon as GunController;
    }
    public override void Attack()
    {
        if (!dead)
        {
            gun.isFiring = true;
            GunFacePlayer();
        }
        else
        {
            gun.isFiring = false;
        }
        
    }

    public override void StopAttacking()
    {
        gun.isFiring = false;
    }

    protected override void Death()
    {
        base.Death();
        //activeWeapon.DropWeapon();
    }
}
