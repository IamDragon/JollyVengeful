using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIAnimator : MonoBehaviour
{
    [SerializeField] Weapon currentWeaponController;

    Vector3 worldDeltaPosition;
    Vector2 groundDeltaPosition;
    Vector2 velocity = Vector2.zero;
    Animator anim;
    NavMeshAgent agent;
    public bool isIdling;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;

    }

    // Update is called once per frame
    void Update()
    {
        SetAnimValues();
    }

    public void SetAnimValues()
    {
        worldDeltaPosition = agent.nextPosition - transform.position;
        groundDeltaPosition.x = Vector3.Dot(transform.right, worldDeltaPosition);
        groundDeltaPosition.y = Vector3.Dot(transform.forward, worldDeltaPosition);
        velocity = (Time.deltaTime > 1e-5f) ? groundDeltaPosition / Time.deltaTime : velocity = Vector2.zero;
        velocity = velocity.normalized;

        if (isIdling)
        {
            velocity.x = 0;
            velocity.y = 0;
        }

        anim.SetFloat("VelocityX", velocity.x);
        anim.SetFloat("VelocityZ", velocity.y);


        if (currentWeaponController is BasicMelee)
            anim.SetLayerWeight(anim.GetLayerIndex("SwordWielding"), 1f);
        else
            anim.SetLayerWeight(anim.GetLayerIndex("SwordWielding"), 0);

        //if(currentWeaponController == null)
            //anim.SetLayerWeight(anim.GetLayerIndex("SwordWielding"), 1f);


    }

    private void OnAnimatorMove()
    {
        transform.position = agent.nextPosition;
    }


}
