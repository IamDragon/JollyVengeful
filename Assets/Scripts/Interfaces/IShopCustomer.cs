using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopCustomer 
{
    Transform GetTransform();

    void BoughtItem(ShopItem.ItemType itemType);
    void BoughtWeapon(Weapon weapon);
    void SpendGoldAmount(int spendGoldAmount);

    bool CheckEnoughGold(int spendGoldAmount);
    bool CanIncreaseGrenades();
}
