using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask whatIsGround;
    [SerializeField] LayerMask whatIsPlayer;
    Transform player;

    //patroling
    public Vector3 walkPoint;
    protected bool walkPointSet;
    public float walkPointRange;


    //states
    [SerializeField] float sightRange, attackRange, attackBufferRange;
    [SerializeField] bool playerInSightRange, playerInAttackRange, playerInBufferRange;

    public bool idle;
    [SerializeField] float idleTime;

    [SerializeField] bool isIdling;
    [SerializeField] float idlingFor;

    [SerializeField] Enemy attackType;

    AIAnimator aiAnimator;

    protected virtual void Awake()
    {
        //player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    protected virtual void Start()
    {
        player = PlayerController.instance.transform;
        aiAnimator = GetComponent<AIAnimator>();
    }

    protected virtual void Update()
    {
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInBufferRange = Physics.CheckSphere(transform.position, attackBufferRange, whatIsPlayer);

        if (playerInSightRange && playerInAttackRange)
            AttackPlayer();
        else if (playerInSightRange && playerInBufferRange)
            AttackPlayer();
        else if (playerInSightRange && !playerInBufferRange)
            ChasePlayer();
        else if (!playerInSightRange && !playerInBufferRange)
            Patroling();

        aiAnimator.isIdling = isIdling;
    }

    private void Patroling()
    {
        if (!isIdling)
        {
            if (!walkPointSet)
                SearchWalkPoint();
            else
                agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
            {
                walkPointSet = false;
                isIdling = true;
                idlingFor = 0;
            }
        }
        else
        {
            if (idlingFor >= idleTime)
                isIdling = false;
            else
                idlingFor += Time.deltaTime;
        }
        gameObject.SendMessage("StopAttacking");
    }

    protected virtual void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        gameObject.SendMessage("StopAttacking");
    }

    private void AttackPlayer()
    {
        transform.LookAt(player);

        gameObject.SendMessage("Attack");

        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackBufferRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
