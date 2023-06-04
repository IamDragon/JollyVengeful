using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : Fighter
{
    public Weapon activeWeapon;
    protected bool alreadyAttacked;
    public Transform player;
    Ragdoll ragdoll;

    public float timeBetweenAttacks;
    [SerializeField] DropPool dropPool;

    protected override void Start()
    {
        base.Start();
        player = PlayerController.instance.transform;
        ragdoll = GetComponent<Ragdoll>();
    }

    protected override void Death()
    {
        
        CombatEvents.EnemyDeath(this);
        if(dropPool)
            dropPool.DropAll();
        activeWeapon.DropWeapon();
        if (ragdoll != null)
        {
            DisableComponents();
            ragdoll.ActivateRagdoll();
            Destroy(gameObject, 10);
        }
        else
        {
            Destroy(gameObject);
        }
        dead = true;
    }

    protected override void ExplosionDeath(float explosionForce, Vector3 forceLocation, float explosionRadius)
    {
        activeWeapon.DropWeapon();

        CombatEvents.EnemyDeath(this);
        if (dropPool)
            dropPool.DropAll();
        if (ragdoll != null)
        {
            DisableComponents();
            ragdoll.ActivateRagdollWithForce(explosionForce, forceLocation, explosionRadius);
            Destroy(gameObject, 10);
        }
        else
        {
            Destroy(gameObject);
        }

        Destroy(gameObject, 10);
        dead = true;

    }

    protected void DisableComponents()
    {
        GetComponent<AIAnimator>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        if (TryGetComponent<PointToPointAI>(out PointToPointAI poinToPoint))
            poinToPoint.enabled = false;
        else if (TryGetComponent<AreaPatrol>(out AreaPatrol areaPatrol))
            areaPatrol.enabled = false;
    }

    public virtual void Attack()
    {

    }
    public virtual void StopAttacking()
    {

    }

    protected virtual void ResetAttack()
    {
        alreadyAttacked = false;
    }

    protected void GunFacePlayer()
    {
        GunController gun = activeWeapon as GunController;
        float playerHeight = 1.25f;
        Vector3 lookAt = new Vector3(player.position.x, player.position.y + playerHeight, player.position.z);
        gun.transform.LookAt(lookAt);
        //gun.firePoint.LookAt(lookAt);

        Ray firingPointRay = new Ray(gun.firePoint.position, Vector3.forward);
        Ray gunRay = new Ray(gun.transform.position, Vector3.forward);
        Ray enemyRay = new Ray(transform.position, Vector3.forward);

        Debug.DrawLine(firingPointRay.origin, lookAt, Color.red);
        Debug.DrawLine(gunRay.origin, lookAt, Color.blue);
        Debug.DrawLine(enemyRay.origin, player.position, Color.green);
    }
}
