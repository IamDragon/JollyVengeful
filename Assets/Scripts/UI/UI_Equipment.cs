using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Equipment : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] Sprite emptySlotIcon;
    [SerializeField] Sprite ammoIcon;

    Image primaryWepImg;
    Image secondaryWepImg;
    Image ammoImg;
    Image throwItemImg;
    Image hpPotImg;

    GameObject primaryWepOverlay;
    GameObject secondaryWepOverlay;

    TextMeshProUGUI inventoryAmmo;
    TextMeshProUGUI throwItemQuantity;
    TextMeshProUGUI hpPotQuantity;

    TextMeshProUGUI primaryWepAmmoCount;
    TextMeshProUGUI secondaryWepAmmoCount;

    // Start is called before the first frame update
    void Start()
    {
        primaryWepImg = GameObject.Find("PrimaryWepIcon").GetComponent<Image>();
        secondaryWepImg = GameObject.Find("SecondaryWepIcon").GetComponent<Image>();
        ammoImg = GameObject.Find("AmmoIcon").GetComponent<Image>();
        throwItemImg = GameObject.Find("ThrowItemIcon").GetComponent<Image>();
        hpPotImg = GameObject.Find("HpPotIcon").GetComponent<Image>();

        inventoryAmmo = GameObject.Find("InventoryAmmo").transform.Find("Quantity").GetComponent<TextMeshProUGUI>();
        throwItemQuantity = GameObject.Find("ThrowItem").transform.Find("Quantity").GetComponent<TextMeshProUGUI>();
        hpPotQuantity = GameObject.Find("HpPot").transform.Find("Quantity").GetComponent<TextMeshProUGUI>();

        primaryWepOverlay = GameObject.Find("PrimaryOverlay");
        secondaryWepOverlay = GameObject.Find("SecondaryOverlay");


        primaryWepAmmoCount = GameObject.Find("PrimaryWepAmmo").GetComponent<TextMeshProUGUI>();
        secondaryWepAmmoCount = GameObject.Find("SecondaryWepAmmo").GetComponent<TextMeshProUGUI>();

        InitializeHUD();
    }

    private void InitializeHUD()
    {
        primaryWepImg.sprite = emptySlotIcon;
        secondaryWepImg.sprite = emptySlotIcon;
        ammoImg.sprite = emptySlotIcon;
        hpPotImg.sprite = emptySlotIcon;
        throwItemImg.sprite = emptySlotIcon;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateActiveWepUI();
        UpdateWeaponUI();
        UpdatePrimaryWepAmmo();
        UpdateSecondaryWepAmmo();

        UpdateInventoryAmmo();
        UpdateGrenadeUI();
        UpdatePotionUI();
    }

    private void UpdateWeaponUI()
    {
        if (inventory.PrimaryWeapon != null)
            primaryWepImg.sprite = inventory.PrimaryWeapon.icon;
        else
            primaryWepImg.sprite = emptySlotIcon;

        if (inventory.SecondaryWeapon != null)
            secondaryWepImg.sprite = inventory.SecondaryWeapon.icon;
        else
            secondaryWepImg.sprite = emptySlotIcon;
    }

    private void UpdateGrenadeUI()
    {
        if (inventory.grenadeCount > 0 && inventory.grenade != null)
        {
            throwItemQuantity.text = inventory.grenadeCount.ToString();
            throwItemImg.sprite = inventory.Grenade.icon;
        }
        else
        {
            throwItemQuantity.text = string.Empty;
            throwItemImg.sprite = emptySlotIcon;
        }
    }

    private void UpdatePotionUI()
    {
        if (inventory.potionCount > 0 && inventory.potion != null)
        {
            hpPotQuantity.text = inventory.potionCount.ToString();
            hpPotImg.sprite = inventory.potion.icon;
        }
        else 
        {
            hpPotQuantity.text = string.Empty;
            hpPotImg.sprite = emptySlotIcon;
        }
    }


    private void UpdatePrimaryWepAmmo()
    {
        if (inventory.PrimaryWeapon is GunController)
        {
            GunController gun = inventory.PrimaryWeapon as GunController;
            primaryWepAmmoCount.text = gun.CurrentMag.ToString() + "/" + gun.MagSize.ToString();
        }
        else
            primaryWepAmmoCount.text = string.Empty;
    }

    private void UpdateSecondaryWepAmmo()
    {
        if (inventory.SecondaryWeapon is GunController)
        {
            GunController gun = inventory.SecondaryWeapon as GunController;
            secondaryWepAmmoCount.text = gun.CurrentMag.ToString() + "/" + gun.MagSize.ToString();
        }
        else
            secondaryWepAmmoCount.text = string.Empty;
    }

    private void UpdateInventoryAmmo()
    {
        if (inventory.CurrentAmmoInInventory <= 0)
        {
            inventoryAmmo.text = string.Empty;
            ammoImg.sprite = emptySlotIcon;
            return;
        }

        inventoryAmmo.text = inventory.CurrentAmmoInInventory.ToString();
        ammoImg.sprite = ammoIcon;
    }

    private void UpdateActiveWepUI()
    {
        if (inventory.CurrentWeapon == null)
        {
            primaryWepOverlay.SetActive(false);
            secondaryWepOverlay.SetActive(false);
            return;
        }

        if (inventory.CurrentWeapon != inventory.PrimaryWeapon && inventory.PrimaryWeapon != null)
            primaryWepOverlay.SetActive(true);
        else if (inventory.CurrentWeapon != inventory.SecondaryWeapon && inventory.SecondaryWeapon != null)
            secondaryWepOverlay.SetActive(true);

        if (inventory.CurrentWeapon == inventory.PrimaryWeapon)
            primaryWepOverlay.SetActive(false);
        else if (inventory.CurrentWeapon == inventory.SecondaryWeapon)
            secondaryWepOverlay.SetActive(false);        
        
    }
}
