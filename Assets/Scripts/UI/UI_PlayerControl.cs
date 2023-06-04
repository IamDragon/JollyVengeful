using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerControl : MonoBehaviour
{
    private GameObject playerControlWindow;

    private void Start()
    {
        playerControlWindow = gameObject.transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (playerControlWindow.activeSelf)
                playerControlWindow.SetActive(false);
            else
                playerControlWindow.SetActive(true);
        }
    }
}
