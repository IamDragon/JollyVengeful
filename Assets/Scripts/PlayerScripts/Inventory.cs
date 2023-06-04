using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IShopCustomer
{
    [SerializeField] Weapon primaryWeapon;
    [SerializeField] Weapon secondaryWeapon;

    [SerializeField] int maxGrenades = 3;
    [SerializeField] public Grenade grenade;
    public int grenadeCount;


    [SerializeField] int maxPotions = 3;
    public HealthPotion potion;
    public int potionCount;

    public int gold = 0;
    public Text goldAmount;

    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentAmmoInInventory;

    public Weapon CurrentWeapon { get; private set; }
    public bool inventoryFull { get; private set; }
    public Weapon PrimaryWeapon => primaryWeapon;
    public Weapon SecondaryWeapon => secondaryWeapon;
    public int CurrentAmmoInInventory => currentAmmoInInventory;
    public Grenade Grenade => grenade;

    [SerializeField] Transform weaponParent;
    public Transform WeaponParent { get { return weaponParent; } }
    public Weapon toPickUp;
    public bool collidingWithWeapon;

    private void Awake()
    {
        if (primaryWeapon != null)
            CurrentWeapon = primaryWeapon;
        else if (secondaryWeapon != null)
            CurrentWeapon = secondaryWeapon;
    }
    private void Start()
    {
        if (CurrentWeapon != null)
        {
            GetComponent<HandPlacementIK>().SetHintLocation(CurrentWeapon.leftHandHandleHint);
            GetComponent<HandPlacementIK>().SetTargetLocation(CurrentWeapon.leftHandHandleTarget);
        }
        PlayerActions.OnWeaponPickUp?.Invoke(CurrentWeapon);
    }
    public Transform GetTransform()
    {
        return transform;
    }

    private void OnEnable()
    {
        PlayerActions.OnHealthPotionPickup += ReceivePotion;
        PlayerActions.OnAmmoPickup += IncreaseAmmo;
    }

    private void OnDisable()
    {
        PlayerActions.OnHealthPotionPickup -= ReceivePotion;
        PlayerActions.OnAmmoPickup -= IncreaseAmmo;
    }

    //Testing inventory sync with HUD when manually inputing gold to inspector
    public void Update()
    {
        UpdateGoldText();
    }

    #region GoldRelated
    public void IncreaseGold(int goldAmount)
    {
        gold += goldAmount;
        UpdateGoldText();
    }
    public void DecreaseGold(int goldAmount)
    {
        gold -= goldAmount;
        UpdateGoldText();
    }

    private void UpdateGoldText()
    {
        goldAmount.text = gold.ToString();
    }

    public void SpendGoldAmount(int spendGoldAmount)
    {
        DecreaseGold(spendGoldAmount);
    }

    public bool CheckEnoughGold(int spendGoldAmount)
    {
        return gold >= spendGoldAmount;
    }

    #endregion

    #region WeaponInteractions

    private void DeactivateCurrentWeapon()
    {
        if (CurrentWeapon != null)
            CurrentWeapon.gameObject.SetActive(false);
    }

    private void ActivateCurrentWeapon()
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.gameObject.SetActive(true);
            SetHandPlacementOnWeapon(CurrentWeapon);
        }
    }

    public void SwitchToPrimary(out Weapon activeWeapon)
    {
        if (primaryWeapon != null)
        {
            DeactivateCurrentWeapon();
            CurrentWeapon = primaryWeapon;
            ActivateCurrentWeapon();
            activeWeapon = CurrentWeapon;
        }
        else
            activeWeapon = null;
    }

    public void SwitchToSecondary(out Weapon activeWeapon)
    {
        if (secondaryWeapon != null)
        {
            DeactivateCurrentWeapon();
            CurrentWeapon = secondaryWeapon;
            ActivateCurrentWeapon();
            activeWeapon = CurrentWeapon;
        }
        else
            activeWeapon = null;
    }

    public void SwapWeapon()
    {
        if (primaryWeapon == null || secondaryWeapon == null)
            return;
        if (CurrentWeapon == primaryWeapon)
        {
            DeactivateCurrentWeapon();
            CurrentWeapon = secondaryWeapon;
            ActivateCurrentWeapon();
        }
        else
        {
            DeactivateCurrentWeapon();
            CurrentWeapon = primaryWeapon;
            ActivateCurrentWeapon();
        }
    }

    public bool CanPickUpWeapon()
    {
        return primaryWeapon == null || secondaryWeapon == null;
    }

    public void OnWeaponPickUp(Weapon newWeapon)
    {
        DeactivateCurrentWeapon();
        if (primaryWeapon == null)
        {
            primaryWeapon = newWeapon;
            CurrentWeapon = primaryWeapon;
        }
        else if (secondaryWeapon == null)
        {
            secondaryWeapon = newWeapon;
            CurrentWeapon = secondaryWeapon;
        }
        ActivateCurrentWeapon();
        PlayerActions.OnWeaponPickUp?.Invoke(CurrentWeapon);
        GetComponent<PlayerWeaponController>().SetActiveWeapon(newWeapon);
    }

    public void OnWeaponRelease()
    {
        if (primaryWeapon == CurrentWeapon)
        {
            primaryWeapon = null;
            CurrentWeapon = null;
        }
        else if (secondaryWeapon == CurrentWeapon)
        {
            secondaryWeapon = null;
            CurrentWeapon = null;
        }
        PlayerActions.OnWeaponDrop?.Invoke(CurrentWeapon);
        PlayerActions.OnReloadEnd?.Invoke();

        ResetActiveWeapon();
    }

    public void ResetActiveWeapon()
    {
        if (primaryWeapon != null)
            CurrentWeapon = primaryWeapon;
        else if (secondaryWeapon != null)
            CurrentWeapon = secondaryWeapon;

        ActivateCurrentWeapon();
        PlayerActions.OnWeaponPickUp(CurrentWeapon);
        GetComponent<PlayerWeaponController>().SetActiveWeapon(CurrentWeapon);
    }

    public void EquipNewWeapon(Weapon newWeapon)
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.DropWeapon();
            OnWeaponRelease();
        }
        Debug.Log(tag);
        Debug.Log(weaponParent);
        newWeapon.EqupiFromShopWeapon(weaponParent, tag, transform.rotation);
        OnWeaponPickUp(newWeapon);
    }

    public void StandingOnWeapon()
    {
        if (Input.GetKey(KeyCode.Space) && collidingWithWeapon && CanPickUpWeapon() && toPickUp != null)
        {
            toPickUp.EquipWeapon(weaponParent, tag, transform.rotation);
            OnWeaponPickUp(toPickUp);
            toPickUp = null;
            SetHandPlacementOnWeapon(CurrentWeapon);
            PlayerActions.gunPickupEvent?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gun"))
        {
            toPickUp = other.gameObject.GetComponent<GunController>();
            collidingWithWeapon = true;
        }
        else if (other.gameObject.CompareTag("Sword"))
        {
            toPickUp = other.gameObject.GetComponent<BasicMelee>();
            collidingWithWeapon = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Gun"))
        {
            toPickUp = null;
            collidingWithWeapon = false;
        }
        else if (other.gameObject.CompareTag("Sword"))
        {
            toPickUp = null;
            collidingWithWeapon = false;
        }
    }

    public int GetReloadAmount(int maxMagCapacity, int currentCapacity)
    {
        if (currentAmmoInInventory >= maxMagCapacity)
        {
            currentAmmoInInventory -= (maxMagCapacity - currentCapacity);
            return maxMagCapacity;
        }
        else if (currentCapacity > 0 && currentAmmoInInventory == 0)
        {

            return currentCapacity;
        }
        else
        {
            int ammoForGun = currentAmmoInInventory;
            currentAmmoInInventory = 0;
            return ammoForGun;
        }
    }
    #endregion

    #region Grenade

    public bool HasGrenades()
    {
        return grenadeCount > 0;
    }

    public void AddGrenade()
    {
        if (CanIncreaseGrenades())
            grenadeCount++;
    }

    public bool CanIncreaseGrenades()
    {
        return grenadeCount < maxGrenades;
    }

    public void RemoveGrenade()
    {
        grenadeCount--;
        if (grenadeCount < 0)
            grenadeCount = 0;
    }
    #endregion

    #region PotionRelated

    public void ReceivePotion()
    {
        if (potionCount < maxPotions)
            potionCount++;
    }

    public bool ConsumePotion(out int healingAmount)
    {
        healingAmount = 0;

        if (potionCount <= 0)
            return false;

        potionCount--;
        healingAmount = potion.HealingAmount;
        return true;
    }
    #endregion

    #region ShopRelated

    public void BoughtItem(ShopItem.ItemType itemType)
    {
        int ammoAmount = 30;
        switch (itemType)
        {
            case ShopItem.ItemType.Grenade: AddGrenade(); break;
            case ShopItem.ItemType.HealthPotion: ReceivePotion(); break;
            case ShopItem.ItemType.Ammo: IncreaseAmmo(ammoAmount); break;
        }
    }

    public void BoughtWeapon(Weapon weapon)
    {
        EquipNewWeapon(weapon);
    }

    private void IncreaseAmmo(int ammoAmount)
    {
        currentAmmoInInventory += ammoAmount;
        if (currentAmmoInInventory > maxAmmo)
            currentAmmoInInventory = maxAmmo;
    }

    #endregion

    public void SetHandPlacementOnWeapon(Weapon weapon)
    {
        if (weapon != null)
        {
            gameObject.GetComponent<HandPlacementIK>().SetTargetLocation(weapon.leftHandHandleTarget);
            gameObject.GetComponent<HandPlacementIK>().SetHintLocation(weapon.leftHandHandleHint);
        }
    }
}
