using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float gravity = 9.8f;
    private Camera mainCamera;
    Vector3 camForward;
    Vector3 camRight;
    private CharacterController myCharacterController;

    private float vSpeed = 0;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        myCharacterController = GetComponent<CharacterController>();
    }

    public void Move(Vector3 direction)
    {
        GetCameraValues();

        direction.Normalize();
        direction *= moveSpeed;

        if (!myCharacterController.isGrounded)
        {
            vSpeed -= gravity * Time.deltaTime;
            direction.y = vSpeed;
        }

        myCharacterController.Move((camForward * direction.z + camRight * direction.x) * Time.deltaTime);
        myCharacterController.Move(new Vector3(0, direction.y, 0));
    }

    private void GetCameraValues()
    {

        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    public void ResetGravity()
    {
        vSpeed = 0;
    }
}
