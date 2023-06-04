using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class BasicMelee : Weapon
{
    [SerializeField] int damageAmount;
    protected BoxCollider hitbox;

    [SerializeField] protected float attackTime;
    protected float attackTimer;
    protected bool attacking;

    protected override void Start()
    {
        base.Start();
        hitbox = GetComponent<BoxCollider>();
        hitbox.enabled = false;
    }

    protected override void Update()
    {
        base.Update();
        if (!isGrounded)
        {
            //Attack();
        }
    }

    public void StartAttack()
    {
        hitbox.enabled = true;
        
        if (!attacking)
        {
            Invoke(nameof(PlaySFX), attackTime / 2);
            Invoke(nameof(ResetAttack), attackTime);
        }
        attacking = true;
    }

    private void ResetAttack()
    {
        hitbox.enabled = false;
        attacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (attacking)
        {
            Fighter fighterObject;
            if (other.gameObject.TryGetComponent<Fighter>(out fighterObject))
            {

                Damage dmg = new Damage
                {
                    damageAmount = damageAmount,
                    origin = transform.position,
                };
                //Damage components wiht enemy tag if the melee is Player tag
                if (other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("PlayerMelee"))
                {
                    fighterObject.SendMessage("ReceiveDamage", dmg);
                }

                //damage components with Player tag if melee tag is Enemy
                else if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("EnemyMelee"))
                {
                    fighterObject.SendMessage("ReceiveDamage", dmg);
                }
            }
        }
    }

    protected override void PlaySFX()
    {
        audioManager.PlaySwordSound();
    }

    protected override void PlayVFX()
    {
        throw new System.NotImplementedException();
    }
}
