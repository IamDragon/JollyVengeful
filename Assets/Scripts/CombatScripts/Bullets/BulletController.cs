using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float speed;
    public int damageAmount;
    public float ttl;
    private float timeAlive;
    private AudioManager audioManager;

    [SerializeField] ParticleSystem bloodShot;

    private void Awake()
    {
    }
    void Start()
    {
        audioManager = AudioManager.instance;
        Invoke(nameof(DestroySelf), ttl);

    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    protected virtual void PlayFX()
    {
        PlayVFX();
        PlaySFX();
    }

    protected virtual void PlayVFX()
    {
        ParticleSystem newBLood = Instantiate(bloodShot, transform.position, transform.rotation) as ParticleSystem;
        newBLood.Play();
    }

    protected virtual void PlaySFX()
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Fighter fighterObject;
        if (other.gameObject.TryGetComponent<Fighter>(out fighterObject))
        {
            audioManager.Play("HitMarker");
            Damage dmg = new Damage
            {
                damageAmount = damageAmount,
                origin = transform.position,
            };
            //Damage components wiht enemy tag if the bullet is Player tag
            if (other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("PlayerBullet"))
            {
                fighterObject.SendMessage("ReceiveDamage", dmg);
                Debug.Log(other.name);
                PlayFX();

                Destroy(gameObject);
            }

            //damage components with Player tag if bullet tag is Enemy
            else if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("EnemyBullet"))
            {
                fighterObject.SendMessage("ReceiveDamage", dmg);
                PlayFX();

                Destroy(gameObject);
            }
            if (other.gameObject.CompareTag("NPC"))
            {
                fighterObject.SendMessage("ReceiveDamage", dmg);
                Debug.Log(other.name);
                PlayFX();
                Destroy(gameObject);
            }
        }
       

        if (other.gameObject.CompareTag("Object"))
        {
            Destroy(gameObject);
        }
        


    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            Destroy(gameObject);
        }
    }


    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
