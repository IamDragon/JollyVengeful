using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public enum ItemType
    {
        Shotgun,
        AssaultRifle,
        BurstPistol,
        StartingPistol,
        CutlassSword,
        HealthPotion,
        Grenade,
        Ammo
    }

    public Item item;
    public int cost;
    public string itemName;
    public ItemType itemType;
}

public class ShopItems : MonoBehaviour
{
    public ShopItem[] shopItems;


    /*
    public Shotgun NewShotGun()
    {
        Shotgun newShotgun = Instantiat
    }
    */
}
