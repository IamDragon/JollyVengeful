using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Shop : MonoBehaviour
{
    [SerializeField] ShopItems shopItems;
    [SerializeField] Transform shopItemTemplate;
    private IShopCustomer shopCustomer;

    Button shopButton;
      
    private void Awake()
    {
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        for (int i = 0; i < shopItems.shopItems.Length; i++)
            CreateItemButton(shopItems.shopItems[i].item.icon, shopItems.shopItems[i].itemName, shopItems.shopItems[i].cost, i);

        Hide();
    }

    private void CreateItemButton(Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate);
        shopItemTransform.SetParent(shopItemTemplate.parent, false);
        shopItemTransform.gameObject.SetActive(true);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        shopItemTransform.Find("itemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        shopItemTransform.Find("itemIcon").GetComponent<Image>().sprite = itemSprite;

        shopButton = shopItemTransform.GetComponent<Button>();
        shopButton.onClick.AddListener(() => { TryBuyItem(positionIndex); });
    }

    public void TryBuyItem(int index)
    {

        if (shopCustomer.CheckEnoughGold(shopItems.shopItems[index].cost))
        {
            Transform playerTransform = shopCustomer.GetTransform();
            shopCustomer.SpendGoldAmount(shopItems.shopItems[index].cost);
            if (shopItems.shopItems[index].itemType == ShopItem.ItemType.Shotgun)
            {
                Shotgun pew = Instantiate(shopItems.shopItems[index].item, playerTransform.position, playerTransform.rotation) as Shotgun;
                shopCustomer.BoughtWeapon(pew);
            }
            else if (shopItems.shopItems[index].itemType == ShopItem.ItemType.AssaultRifle)
            {
                GunController pew = Instantiate(shopItems.shopItems[index].item, playerTransform.position, playerTransform.rotation) as GunController;
                shopCustomer.BoughtWeapon(pew);
            }
            else if (shopItems.shopItems[index].itemType == ShopItem.ItemType.BurstPistol)
            {
                BurstGun pew = Instantiate(shopItems.shopItems[index].item, playerTransform.position, playerTransform.rotation) as BurstGun;
                shopCustomer.BoughtWeapon(pew);
            }
            else if (shopItems.shopItems[index].itemType == ShopItem.ItemType.StartingPistol)
            {
                BurstGun pew = Instantiate(shopItems.shopItems[index].item, playerTransform.position, playerTransform.rotation) as BurstGun;
                shopCustomer.BoughtWeapon(pew);
            }
            else if (shopItems.shopItems[index].itemType == ShopItem.ItemType.CutlassSword)
            {
                BasicMelee cutlass = Instantiate(shopItems.shopItems[index].item, playerTransform.position, playerTransform.rotation) as BasicMelee;
                shopCustomer.BoughtWeapon(cutlass);
            }
            else if (shopItems.shopItems[index].itemType == ShopItem.ItemType.HealthPotion)
            {
                shopCustomer.BoughtItem(shopItems.shopItems[index].itemType);
            }
            else if (shopItems.shopItems[index].itemType == ShopItem.ItemType.Grenade && shopCustomer.CanIncreaseGrenades() == true)
            {
                shopCustomer.BoughtItem(shopItems.shopItems[index].itemType);
            }
            else if (shopItems.shopItems[index].itemType == ShopItem.ItemType.Ammo)
            {
                shopCustomer.BoughtItem(shopItems.shopItems[index].itemType);
            }
        }
    }

    public void Show(IShopCustomer shopCustomer)
    {
        GameManager.instance.SetShowCursor(true);
        gameObject.SetActive(true);
        this.shopCustomer = shopCustomer;
    }

    public void Hide()
    {
        GameManager.instance.SetShowCursor(false);
        gameObject.SetActive(false);
    }
}
