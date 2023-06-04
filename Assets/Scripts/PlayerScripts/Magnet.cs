using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] float magnetStrength = 15;
    private void Start()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<Collectable>(out Collectable collectable))
            collectable.SetTarget(transform.parent.position, magnetStrength);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<GoldCoin>(out GoldCoin coin))
        {
            coin.RemoveTarget();
        }
    }
}
