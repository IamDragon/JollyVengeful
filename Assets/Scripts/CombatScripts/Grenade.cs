using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Weapon
{
    [SerializeField] ParticleSystem explosion;
    [SerializeField] int damageAmount;
    [SerializeField] float timeBeforeExplosion;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionForce = 500;
    [SerializeField] LayerMask whatToDamage;


    public bool isActivated;
    private float explosionTimeCounter;

    protected override void Update()
    {
        base.Update();
        //if one of these is true it is not being held by anyone
        if (isActivated)
            ExplosionTimer();
    }

    private void ExplosionTimer()
    {
        explosionTimeCounter += Time.deltaTime;
        //timer to give gun time to be thrown before checking if not moving
        if (explosionTimeCounter >= timeBeforeExplosion)
        {
            Explode();
        }
    }

    private void Explode()
    {
        PlayFX();
        audioManager.Play("GrenadeExplosion");
        DamageEntities();
        Destroy(gameObject);
    }

    private void AddForce(Collider[] colliders)
    {
        foreach (Collider closeObjects in colliders)
        {
            Rigidbody rigidbody = closeObjects.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }

    private void DamageEntities()
    {
        Collider[] entities = Physics.OverlapSphere(transform.position, explosionRadius, whatToDamage);
        AddForce(entities);
        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i].TryGetComponent<Fighter>(out Fighter fighterObject))
            {
                Damage dmg = new Damage
                {
                    damageAmount = damageAmount,
                    origin = transform.position,
                };
                //fighterObject.SendMessage("ReceiveDamage", dmg, 500f, transform.position, explosionRadius);
                fighterObject.ReceiveExplosionDamage(dmg, explosionForce, transform.position, explosionRadius);
            }
        }
    }
    protected override void PlaySFX()
    {
        //Play explosionSound
    }
    protected override void PlayVFX()
    {
        ParticleSystem explosionFX = Instantiate(explosion, transform.position, transform.rotation) as ParticleSystem;
        explosionFX.Play();
    }
    public override void ThrowWeapon()
    {
        base.ThrowWeapon();
        isActivated = true;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isActivated)
        {
            if (other.gameObject.TryGetComponent<Inventory>(out Inventory inventory) && inventory.CanIncreaseGrenades())
            {
                inventory.AddGrenade();
                PlayerActions.OnGrenadePickup?.Invoke();
                Destroy(gameObject);
            }
        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
         if (!collision.gameObject.CompareTag("Player"))
        {
            audioManager.Play("GrenadeImpact");
        }
    }
}