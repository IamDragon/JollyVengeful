using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class CheckLocation : MonoBehaviour
{
    private Transform moveObject;
    private GameObject questLog;

    public void Start()
    {
        moveObject = this.transform;
        questLog = QuestList.instance.gameObject;
    }
    public void OnTriggerEnter(Collider other)
    {
       
        if (other.name=="Player")
        {           
            questLog.GetComponent<QuestList>().CheckLocation(moveObject);
        }
    }
}
