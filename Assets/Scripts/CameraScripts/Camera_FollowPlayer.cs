using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_FollowPlayer : MonoBehaviour
{
    public GameObject player;
    Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        //transform.position = player.transform.position + new Vector3(6.458f, 6.609f, -7.131f);
        transform.position = player.transform.position + startPosition;
    }
}
