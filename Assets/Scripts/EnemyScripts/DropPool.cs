using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPool : MonoBehaviour
{
    [SerializeField] bool dropActiveWeapon;
    private Weapon activeWeapon;

    [SerializeField] bool dropGrenades;
    [SerializeField] Grenade grenade;
    [SerializeField] int maxGrenades;
    [SerializeField] int minGrenades;

    [SerializeField] bool dropGoldCoins;
    [SerializeField] GoldCoin goldCoin;
    [SerializeField] int maxGoldCoins;
    [SerializeField] int minGoldCoins;

    [SerializeField] bool dropHealthpoitions;
    [SerializeField] HealthPotion healthPotion;
    [SerializeField] int maxHealthPotions;
    [SerializeField] int minHealthPotions;

    private int itemBaseHeightSpawn = 1;

    private void Start()
    {
        activeWeapon = GetComponent<Enemy>().activeWeapon;
    }
    public void DropAll()
    {
        if (dropActiveWeapon)
            DropActiveWeapon();
        if (dropGrenades)
            DropGrenades();
        if (dropGoldCoins)
            DropGoldCoins();
        if (dropHealthpoitions)
            DropHealthPotions();
    }

    private void DropActiveWeapon()
    {
        activeWeapon.DropWeapon();
    }

    private void DropGrenades()
    {
        int grenadesToDrop = Random.Range(minGrenades, maxGrenades);
        for (int i = 0; i < grenadesToDrop; i++)
        {
            Instantiate(grenade, transform.position, transform.rotation);

        }
    }

    private void DropGoldCoins()
    {
        int goldCoinsToDrop = Random.Range(minGoldCoins, maxGoldCoins);
        for (int i = 0; i < goldCoinsToDrop; i++)
        {
            Instantiate(goldCoin, GetItemSpawnLocation(), goldCoin.transform.rotation);

        }
    }

    private void DropHealthPotions()
    {
        int healthPotionsToDrop = Random.Range(minHealthPotions, maxHealthPotions);
        for (int i = 0; i < healthPotionsToDrop; i++)
        {

            Instantiate(healthPotion, GetItemSpawnLocation(), healthPotion.transform.rotation);

        }
    }

    private Vector3 GetItemSpawnLocation()
    {
        float maxDistanceToOrigin = 1.5f;
        Vector3 spawnLocation = new Vector3(
            transform.position.x + Random.Range(0, maxDistanceToOrigin),
            transform.position.y + itemBaseHeightSpawn,
            transform.position.z + Random.Range(0, maxDistanceToOrigin)
            );

        
        RaycastHit hit;
        if (Physics.Raycast(spawnLocation, Vector3.down, out hit))
        {
            spawnLocation.y = hit.point.y + itemBaseHeightSpawn;
        }
        
        return spawnLocation;
    }
}
