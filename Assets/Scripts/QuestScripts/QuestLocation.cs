using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLocation : MonoBehaviour
{
    private GameObject questLog;
    public Transform locationObject;

    private void Start()
    {
        questLog = QuestList.instance.gameObject;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.name == "Player" && Input.GetKeyDown(KeyCode.F))
        {
            questLog.GetComponent<QuestList>().CheckLocation(locationObject);
        }
    }
}
