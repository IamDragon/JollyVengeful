using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Collectable
{
    [SerializeField] int healingAmount;

    public int HealingAmount => healingAmount;

    protected override void Spin()
    {
        transform.Rotate(0, rotationSpeed, rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerActions.OnHealthPotionPickup?.Invoke();
            Destroy(gameObject);
        }
    }
}
