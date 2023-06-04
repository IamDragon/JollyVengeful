using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Animations.Rigging;
using TMPro;
public class PlayerController : MonoBehaviour
{
    [SerializeField] float gravity = 9.8f;
    [SerializeField] Transform aimTargetTransform;

    [SerializeField] float grenadeCooldown;

    public HandPlacementIK handPlacementIK;

    private CharacterController myCharacterController;
    public Vector3 moveInput;
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;
    private PlayerWeaponController weaponController;
    private PlayerAimController aimController;

    private float currentGravity;
    private float startHeight;

    public bool active { get; set; }

    Animator myAnimator;

    private Camera mainCamera;

    private bool canThrowGrenade;
    public bool isReloading;
    private Inventory inventory;
    private PlayerStatus playerStatus;

    private CapsuleCollider capCollider;

    public static PlayerController instance;

    AudioManager audioManager;

    Vector3 spawnPos;

    private void Awake()
    {
        active = true;

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        audioManager = AudioManager.instance;
        mainCamera = FindObjectOfType<Camera>();
        myAnimator = GetComponent<Animator>();
        myCharacterController = GetComponent<CharacterController>();
        capCollider = GetComponent<CapsuleCollider>();
        startHeight = myCharacterController.height;
        transform.tag = "Player";

        inventory = GetComponent<Inventory>();
        playerStatus = GetComponent<PlayerStatus>();

        UpdateGrenadeStatus();

        currentGravity = gravity;
        handPlacementIK = GetComponent<HandPlacementIK>();

        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        weaponController = GetComponent<PlayerWeaponController>();
        aimController = GetComponent<PlayerAimController>();
        GameManager.instance.SetShowCursor(false);
    }

    private void OnEnable()
    {
        PlayerActions.OnGrenadePickup += UpdateGrenadeStatus;

        SpawnManager.instance.SpawnPlayer();

        if (inventory != null && playerAnimation != null)
            playerAnimation.SetWalkingAnimation(inventory.CurrentWeapon);

    }

    private void OnDisable()
    {
        PlayerActions.OnGrenadePickup -= UpdateGrenadeStatus;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            Loader.Load(Loader.Scene.Harbor);
        }
        if (active)
        {
            moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            playerMovement.Move(moveInput);
            aimController.Aim(aimTargetTransform);
            SetAimTransformHeight();


            if (playerStatus.ShopOpen == false)
            {
                weaponController.CheckInputs();
                inventory.StandingOnWeapon();
                ThrowGrenade();
                DrinkPotion();
            }
        }
        playerAnimation.Animation(moveInput);
        playerAnimation.UpdateHandPlacement(inventory.CurrentWeapon);


    }
    private void LateUpdate()
    {
        UpdatePlayerCollidersWhenCrouching();
    }


    private void UpdatePlayerCollidersWhenCrouching()
    {
        float newHeight = startHeight;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            newHeight = 0.7f * startHeight;
            capCollider.center = new Vector3(0, .7f, 0);
            if (myCharacterController.center.y > 0.6f)
                myCharacterController.center -= new Vector3(0, 1.5f * Time.deltaTime, 0);
        }
        else
        {
            capCollider.center = new Vector3(0, 0.9f, 0);

            if (myCharacterController.center.y < 0.93f)
                myCharacterController.center += new Vector3(0, 1.5f * Time.deltaTime, 0);
        }

        float lastHeight = myCharacterController.height;

        capCollider.height = Mathf.Lerp(capCollider.height, newHeight, 5.0f * Time.deltaTime);
        myCharacterController.height = Mathf.Lerp(myCharacterController.height, newHeight, 5.0f * Time.deltaTime);

        transform.position += new Vector3(0, (myCharacterController.height - lastHeight) * 0.5f, 0);

    }

    void SetAimTransformHeight()
    {
        if (aimTargetTransform != null)
        {
            if (myAnimator.GetBool("isCrouching"))
            {
                aimTargetTransform.position = new Vector3(aimTargetTransform.position.x, transform.position.y + 1f, aimTargetTransform.position.z);
            }
            else
                aimTargetTransform.position = new Vector3(aimTargetTransform.position.x, transform.position.y + 1.6f, aimTargetTransform.position.z);
        }
        else
        {
            Debug.LogWarning("AimTransform is null");
        }

    }

    private void UpdateGrenadeStatus()
    {
        if (inventory.HasGrenades())
            canThrowGrenade = true;
    }

    private void ThrowGrenade()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (inventory.HasGrenades() && canThrowGrenade)
            {
                Debug.Log("Throw grenade");
                Invoke(nameof(ResetGrenade), grenadeCooldown);
                canThrowGrenade = false;
                Grenade newGrenade = Instantiate(inventory.grenade, inventory.WeaponParent.position, transform.rotation);
                newGrenade.ThrowWeapon();
                audioManager.Play("ThrowGrenade");
                inventory.RemoveGrenade();
            }
        }
    }

    private void ResetGrenade()
    {
        canThrowGrenade = true;
    }

    private void DrinkPotion()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) && !playerStatus.IsMaxHealth())
            if (inventory.ConsumePotion(out int healingAmount))
                playerStatus.ReceiveHealing(healingAmount);
    }

    public void SetCamera(Camera camera)
    {
        mainCamera = camera;
    }

    public void SetPos(Vector3 position)
    {
        
        GetComponent<CharacterController>().enabled = false;
        transform.position = new Vector3(position.x, position.y, position.z);
        GetComponent<CharacterController>().enabled = true;
    }
}