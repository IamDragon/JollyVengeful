using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombThrower : Enemy
{
    [SerializeField] private Grenade grenade;
    public override void Attack()
    {
        if (!dead)
        {
            if (!alreadyAttacked)
            {
                alreadyAttacked = true;
                Grenade newGrenade = Instantiate(grenade, transform.position, transform.rotation);
                newGrenade.ThrowWeapon();
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
       
    }

    protected override void Death()
    {
        base.Death();
        Instantiate(grenade, transform.position, transform.rotation);
    }
}
