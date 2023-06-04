using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class PlayerActions
{
    public static Action OnShopOpen;
    public static Action OnShopClose;
    public static Action OnHealthPotionPickup;

    public static Action OnGrenadePickup;
    public static Action<int> OnAmmoPickup;
    
    public static Action<Weapon> OnWeaponPickUp;
    public static Action<Weapon> OnWeaponDrop;
    public static Action OnReloadStart;
    public static Action OnReloadEnd;

    public static Action gunPickupEvent;
}
