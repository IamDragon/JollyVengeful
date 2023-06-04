using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Collectable
{

    [SerializeField] private int minAmmo;
    [SerializeField] private int maxAmmo;
    private int ammoAmount;

    protected override void Start()
    {
        base.Start();
        ammoAmount = Random.Range(minAmmo, maxAmmo);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(ammoAmount);
            PlayerActions.OnAmmoPickup?.Invoke(ammoAmount);
            Destroy(gameObject);
        }
    }
}
