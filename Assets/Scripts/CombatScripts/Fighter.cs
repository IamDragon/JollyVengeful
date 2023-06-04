using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public string enemyType;
    public bool invincible;
    public bool dead = false;
    protected virtual void Start()
    {
    }


    protected virtual void ReceiveDamage(Damage dmg)
    {
        health -= dmg.damageAmount;
        if (invincible)
            health = maxHealth;
        else if (health <= 0)
        {
            health = 0;
            Death();
        }
    }
    public void ReceiveExplosionDamage(Damage dmg, float explosionForce, Vector3 forceLocation, float explosionRadius)
    {
        health -= dmg.damageAmount;
        if (invincible)
            health = maxHealth;
        else if (health <= 0)
        {
            health = 0;
            ExplosionDeath(explosionForce, forceLocation, explosionRadius);
        }
    }
    public virtual void ReceiveHealing(int healing)
    {
        health = health + healing;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    protected virtual void Death()
    {
        

    }
    protected virtual void ExplosionDeath(float explosionForce, Vector3 forceLocation, float explosionRadius)
    {
    }
}
