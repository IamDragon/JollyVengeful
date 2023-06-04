using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]

public abstract class Weapon : Item
{


    [SerializeField] protected bool isGrounded;
    [SerializeField] protected bool isAirborne;
    [SerializeField] protected int throwDamage;
    [SerializeField] protected Vector3 heldPosition;

    [SerializeField] protected float throwForceForward;
    [SerializeField] protected float throwForceUpwards;
    [SerializeField] protected Vector3 throwTorque;
    [SerializeField] protected GameObject raycast;
    [SerializeField] protected ParticleSystem groundedFX;
    public Transform leftHandHandleTarget;
    public Transform leftHandHandleHint;
    MeshCollider mc;


    protected Rigidbody rb;

    protected float throwSpeed;
    protected float totalAirTime;
    protected float timeInAir;
    protected AudioManager audioManager;

    private void Awake()
    {
    }
    protected virtual void Start()
    {
        audioManager = AudioManager.instance;

        mc = GetComponent<MeshCollider>();
        if (mc != null)
        {
            mc.enabled = true;
        }
        FindRigidBody();
        totalAirTime = 0.5f;
        if (!transform.parent)
        {
            isAirborne = true;
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        else
        {
            if (transform.parent.CompareTag("Enemy"))
                tag = transform.parent.tag;
            else if (transform.parent.CompareTag("Player"))
                SetTagOnPickup(transform.parent.tag);
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    protected virtual void Update()
    {
        if (!isGrounded)
            WhileAirborne();
    }

    protected virtual void PlayFX()
    {
        PlaySFX();
        PlayVFX();
    }

    protected abstract void PlaySFX();
    protected abstract void PlayVFX();
    private void PlayGroundedVFX()
    {
        if (!groundedFX)
            return;
        groundedFX.gameObject.SetActive(true);
        groundedFX.Play();
    }

    private void StopGroundedVFX()
    {
        if (!groundedFX)
            return;
        groundedFX.Stop();
        groundedFX.gameObject.SetActive(false);
    }

    public virtual void DropWeapon()
    {
        FindRigidBody();
        RemoveParent();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            timeInAir = 0;
            isGrounded = false;
            isAirborne = true;
        }
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }

    public virtual void ThrowWeapon()
    {
        if (tag != "Item")
            tag = transform.parent.tag + "Bullet";
        DropWeapon();
        if (rb != null)
        {
            rb.AddForce(transform.forward * throwForceForward + transform.up * throwForceUpwards);
            rb.AddTorque(throwTorque);

        }
        isAirborne = true;

    }

    private void RemoveParent()
    {
        transform.parent = null;
    }

    protected void WhileAirborne()
    {
        if (isAirborne)
        {
            timeInAir += Time.deltaTime;
            //timer to give gun time to be thrown before checking if not moving
            if (timeInAir > totalAirTime)
            {
                //assume gun is grounded when no motion is applied
                if (rb.velocity == Vector3.zero)
                {
                    isAirborne = false;
                    isGrounded = true;
                    rb.isKinematic = true;
                    rb.useGravity = false;
                    if (this is BasicMelee)
                        tag = "Sword";
                    else if (this is GunController)
                        tag = "Gun";
                    PlayGroundedVFX();
                }
            }

        }
    }

    public virtual void EquipWeapon(Transform parent, string newTag, Quaternion rotation)
    {
        if (isGrounded)
        {
            transform.SetParent(parent);
            transform.localPosition = heldPosition;
            transform.rotation = rotation;
            if (this is BasicMelee)
            {
                transform.localRotation = Quaternion.Euler(3f, 87f, -81f);

            }
            isGrounded = false;
            SetTagOnPickup(newTag);
            if(groundedFX.isPlaying)
                StopGroundedVFX();
        }
    }

    public void EqupiFromShopWeapon(Transform parent, string newTag, Quaternion rotation)
    {
        transform.SetParent(parent);
        transform.localPosition = heldPosition;
        transform.rotation = rotation;
        if (this is BasicMelee)
        {
            transform.localRotation = Quaternion.Euler(3f, 87f, -81f);
        }
        isGrounded = false;
        SetTagOnPickup(newTag);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Fighter fighterObject;
        if (collision.gameObject.TryGetComponent<Fighter>(out fighterObject))
        {
            Damage dmg = new Damage
            {
                damageAmount = throwDamage,
                origin = transform.position,
            };
            //Damage components with enemy tag if the bullet is Player tag
            if (collision.gameObject.CompareTag("Enemy") && gameObject.CompareTag("PlayerBullet"))
            {
                fighterObject.SendMessage("ReceiveDamage", dmg);
            }

            //damage components with Player tag if bullet tag is Enemy
            else if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("EnemyBullet"))
            {
                fighterObject.SendMessage("ReceiveDamage", dmg);
            }
        }

    }

    public void FindRigidBody()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void SetTagOnPickup(string newTagBase)
    {
        if (this is BasicMelee)
            gameObject.tag = newTagBase + "Melee";
        else if (this is GunController)
            gameObject.tag = newTagBase + "Gun";
    }

    public Transform GetRaycast()
    {
        if (raycast != null)
        {
            return raycast.transform;
        }
        else
        {
            return null;
        }
    }
}
