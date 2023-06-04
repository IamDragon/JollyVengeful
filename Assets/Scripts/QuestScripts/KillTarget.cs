using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class KillTarget : MonoBehaviour
{
    public string enemyType;
    public int hp;
    public void OnTriggerEnter(Collider other)
    {
        hp--;
        if(hp<=0)
        {
            Destroy(gameObject);
        }
    }
}
