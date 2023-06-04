using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToPointAI : EnemyPatrolAI
{
    public Vector3[] walkPoints;
    private int nextDestination;

    protected override void SearchWalkPoint()
    {
        nextDestination++;
        if (nextDestination >= walkPoints.Length)
            nextDestination = 0;
        walkPoint = walkPoints[nextDestination];

        walkPointSet = true;
    }
}
