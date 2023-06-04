using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Enemy
{
    public override void Attack()
    {

    }
    public override void StopAttacking()
    {
        
    }   
    protected override void Death()
    {
        CombatEvents.EnemyDeath(this);
        Destroy(gameObject);
    }
}
