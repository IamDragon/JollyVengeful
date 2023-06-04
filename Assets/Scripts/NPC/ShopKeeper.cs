using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ShopKeeper : MonoBehaviour
{
    [SerializeField] Canvas openShopCanvas;
    [SerializeField] UI_Shop shopUI;    
    private bool playerInRange;
    private bool shopOpen;
    private IShopCustomer shopCustomer;
    private Transform player;
    private Text shopKeeperText;

    private void Start()
    {
        SetShopOverhead();
    }

    private void Update()
    {
        InRange();
    }

    private void InRange()
    {
        if (playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E) && !shopOpen)
                OpenShop();
            else if (Input.GetKeyDown(KeyCode.E) && shopOpen)
                CloseShop();
        }
    }

    private void OpenShop()
    {
        if (shopCustomer != null)
        {
            shopUI.Show(shopCustomer);
            shopOpen = true;
            openShopCanvas.enabled = false;
            PlayerActions.OnShopOpen?.Invoke();
        }
    }

    private void CloseShop()
    {
        shopUI.Hide();
        shopOpen = false;
        openShopCanvas.enabled = true;
        PlayerActions.OnShopClose?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            openShopCanvas.enabled = true;
            other.TryGetComponent<IShopCustomer>(out shopCustomer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (shopOpen)
            {
                CloseShop();
            }
            playerInRange = false;
            openShopCanvas.enabled = false;
        }
    }
    private void SetShopOverhead()
    {
        Font arial;
        arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

        GameObject canvasGO = new GameObject();
        canvasGO.name = "ShopKeeperCanvas";
        canvasGO.AddComponent<Canvas>();
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        Canvas canvas;
        canvas = canvasGO.GetComponent<Canvas>();
        canvas.transform.position = new Vector3(this.transform.position.x,
            this.transform.position.y - 40, this.transform.position.z);
        canvas.scaleFactor = 3000;

        GameObject textGo = new GameObject();
        textGo.transform.parent = canvasGO.transform;
        textGo.AddComponent<Text>();

        shopKeeperText = textGo.GetComponent<Text>();
        shopKeeperText.font = arial;
        shopKeeperText.fontSize = 1;
        shopKeeperText.color = Color.yellow;
        shopKeeperText.transform.position = new Vector3(this.transform.position.x,
            this.transform.position.y - 48, this.transform.position.z);
        shopKeeperText.alignment = TextAnchor.UpperCenter;
        shopKeeperText.text = "Shop Keeper";

    }
}
