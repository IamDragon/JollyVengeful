using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTavern : MonoBehaviour
{
    [SerializeField] Transform enterObject;
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Loader.Load(Loader.Scene.Harbor);
        }
    }
}
