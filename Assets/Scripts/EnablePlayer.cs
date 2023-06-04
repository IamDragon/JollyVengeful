using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlayer : MonoBehaviour
{
    bool playerEnabled = false;
    bool hudEnabled = false;
    private void Awake()
    {
       
    }

    void Start()
    {
        
    }
    void Update()
    {
        if (!playerEnabled)
        {
            if (PlayerController.instance != null)
            {
                PlayerController.instance.gameObject.SetActive(true);
                playerEnabled = true;
            }
        }

        if (!hudEnabled)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.HudToggle(true);
                hudEnabled = true;
            }
        }

    }
}
