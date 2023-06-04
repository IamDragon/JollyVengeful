using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuestLog : MonoBehaviour
{
    private GameObject questLog;

    private void Start()
    {
        questLog = gameObject.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (questLog.activeSelf)
                questLog.SetActive(false);
            else
                questLog.SetActive(true);
        }
    }
}
