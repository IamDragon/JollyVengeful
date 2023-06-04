using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerController playerController;
    private Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;
    private Animator myAnimator;

    private HandPlacementIK handPlacementIK;
    private WeaponIK weaponIK;

    private delegate void AnimationDel();
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        handPlacementIK = GetComponent<HandPlacementIK>();
    }
    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        playerController = GetComponent<PlayerController>();
        weaponIK = GetComponent<WeaponIK>();
    }
    private void OnEnable()
    {
        PlayerActions.OnWeaponPickUp += SetWeaponWielding;
        PlayerActions.OnWeaponPickUp += SetWalkingAnimation;
        PlayerActions.OnWeaponDrop += SetWeaponWielding;
        PlayerActions.OnWeaponDrop += SetWalkingAnimation;
        PlayerActions.OnReloadStart += StartReloadAnimation;
        PlayerActions.OnReloadEnd += StopReloadAnimation;
    }

    private void OnDisable()
    {
        PlayerActions.OnWeaponPickUp -= SetWeaponWielding;
        PlayerActions.OnWeaponPickUp -= SetWalkingAnimation;
        PlayerActions.OnWeaponDrop -= SetWeaponWielding;
        PlayerActions.OnWeaponDrop -= SetWalkingAnimation;
        PlayerActions.OnReloadStart -= StartReloadAnimation;
        PlayerActions.OnReloadEnd -= StopReloadAnimation;
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

    public void Animation(Vector3 moveInput)
    {
        if (!playerController.active)
            moveInput = Vector3.zero;

        GetCameraValues();

        float velocityZ = Vector3.Dot((camForward * moveInput.z + camRight * moveInput.x).normalized, transform.forward);
        float velocityX = Vector3.Dot((camForward * moveInput.z + camRight * moveInput.x).normalized, transform.right);

        myAnimator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        myAnimator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);

        myAnimator.SetBool("isCrouching", Input.GetKey(KeyCode.LeftShift));

    }

    private void StartReloadAnimation()
    {
        handPlacementIK.SetRigWeight(0);
        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex("Reload"), 1);
        myAnimator.SetBool("isReloading", true);
    }

    private void StopReloadAnimation()
    {
        myAnimator.SetBool("isReloading", false);
        if (myAnimator.GetLayerWeight(myAnimator.GetLayerIndex("Reload")) > 0)
        {
            myAnimator.SetLayerWeight(myAnimator.GetLayerIndex("Reload"), 
                myAnimator.GetLayerWeight(myAnimator.GetLayerIndex("Reload")) - (4f * Time.deltaTime));
        }
    }

    public void SetWalkingAnimation(Weapon weapon)
    {
        if (weapon == null || weapon is BasicMelee)
        {
            myAnimator.SetLayerWeight(myAnimator.GetLayerIndex("Walking"), .7f);
            if (handPlacementIK.GetRigWeight() > 0)
                handPlacementIK.SetRigWeight(handPlacementIK.GetRigWeight() - (4f * Time.deltaTime));
            
        }
        else
        {
            myAnimator.SetLayerWeight(myAnimator.GetLayerIndex("Walking"), 0f);
            if (handPlacementIK.GetRigWeight() < 1)            
                handPlacementIK.SetRigWeight(handPlacementIK.GetRigWeight() + (4f * Time.deltaTime));           
        }

    }

    private void SetWeaponWielding(Weapon weapon)
    {
        if (weapon is BasicMelee)
        {
            myAnimator.SetLayerWeight(myAnimator.GetLayerIndex("SwordWielding"), 1f);
            handPlacementIK.SetRigWeight(0);
        }
        else
            myAnimator.SetLayerWeight(myAnimator.GetLayerIndex("SwordWielding"), 0f);
    }

    public void UpdateHandPlacement(Weapon weapon)
    {
        if (weapon == null || weapon is BasicMelee || myAnimator.GetBool("isReloading"))
        {
            if (handPlacementIK.GetRigWeight() > 0)
                handPlacementIK.SetRigWeight(handPlacementIK.GetRigWeight() - (4f * Time.deltaTime));            
        }
        else
        {
            if (handPlacementIK.GetRigWeight() < 1)
                handPlacementIK.SetRigWeight(handPlacementIK.GetRigWeight() + (4f * Time.deltaTime));            
        }
    }
}
