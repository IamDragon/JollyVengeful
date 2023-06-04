using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryNavSystem : MonoBehaviour
{
    [SerializeField] private RectTransform dragRectTransform;

    private Button button;
    private bool isInventoryOpen;
    private GameObject inventoryUI;
    private Vector2 refPosition;

    private void Awake()
    {
        button = GetComponent<Button>();
        button?.onClick.AddListener(UpdateInventoryWindow);

        inventoryUI = GameObject.Find("Inventory");
        refPosition = dragRectTransform.anchoredPosition;
        inventoryUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown("i"))
            UpdateInventoryWindow();
    }

    private void UpdateInventoryWindow()
    {
        if (!isInventoryOpen)
            isInventoryOpen = true;
        else
            isInventoryOpen = false;

        inventoryUI.SetActive(isInventoryOpen);
        dragRectTransform.SetAsLastSibling();
        dragRectTransform.anchoredPosition = refPosition;
    }

    public void ExitInventory()
    {
        isInventoryOpen = false;
        inventoryUI.SetActive(isInventoryOpen);
    }
}
