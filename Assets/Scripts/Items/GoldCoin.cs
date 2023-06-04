using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : Collectable
{
    public int value;

    private void Awake()
    {
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void PickUp()
    {
        base.PickUp();

    }
    protected override void Spin()
    {
        transform.Rotate(0, 0, rotationSpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            other.GetComponent<PlayerStatus>().SendMessage("IncreaseGold", value);
            Destroy(gameObject);
        }
    }
}
