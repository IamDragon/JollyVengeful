using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralNPC : Fighter
{
    Ragdoll ragdoll;
    Animator anim;

    // Update is called once per frame
    private new void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        
    }

    protected override void Death()
    {
        anim.enabled = false;
        ragdoll.ActivateRagdoll();
        DestroyGameObject(5);
    }

    protected override void ExplosionDeath(float explosionForce, Vector3 forceLocation, float explosionRadius)
    {
        anim.enabled = false;
        ragdoll.ActivateRagdollWithForce(explosionRadius, forceLocation, explosionRadius);
        DestroyGameObject(5);
    }

    private void DestroyGameObject(float delay)
    {
        Destroy(this.gameObject, delay);
    }
}
