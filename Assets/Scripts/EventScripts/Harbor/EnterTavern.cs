using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class EnterTavern : MonoBehaviour
{
    [SerializeField] Transform enterObject;
    [SerializeField] Transform exitObject;

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Tag ís correct");
            SetSpawnPoint();
            Loader.Load(Loader.Scene.Tavern);
        }
        
    }

    void SetSpawnPoint()
    {
        SpawnManager.instance.SetSpawn(exitObject.position);
    }
}
