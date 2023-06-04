using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttacker : Enemy
{
    BasicMelee melee;
    public override void Attack()
    {
        GetComponent<Animator>().Play("SwordSlash");
        melee = activeWeapon as BasicMelee;
        melee.StartAttack();
    }

    protected override void Death()
    {
        base.Death();
        if (melee != null)
        {
            melee.DropWeapon();
        }
    }

}
