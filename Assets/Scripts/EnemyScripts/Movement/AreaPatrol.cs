using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaPatrol : EnemyPatrolAI
{
    private Vector3 mainPoint;

    protected override void Awake()
    {
        base.Awake();
        mainPoint = transform.position;
    }

    protected override void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(mainPoint.x + randomX, transform.position.y, mainPoint.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(mainPoint, walkPointRange);
    }
}
