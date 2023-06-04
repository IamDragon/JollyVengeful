using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBullets : BulletController
{
    [SerializeField] ParticleSystem explosion;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionForce = 500;
    [SerializeField] LayerMask whatToDamage;


    protected override void PlayVFX()
    {
        base.PlayVFX();
        ParticleSystem explosionFX = Instantiate(explosion, transform.position, transform.rotation) as ParticleSystem;
        explosionFX.Play();
    }

    protected override void PlaySFX()
    {
        base.PlaySFX();
        //Play explosion Sound
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        Collider[] entities = Physics.OverlapSphere(transform.position, explosionRadius, whatToDamage);
        AddForce(entities);
        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i].TryGetComponent<Fighter>(out Fighter fighterObject))
            {
                Damage dmg = new Damage
                {
                    damageAmount = damageAmount,
                };
                fighterObject.ReceiveExplosionDamage(dmg, explosionForce, transform.position, explosionRadius);
            }
        }
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
}
