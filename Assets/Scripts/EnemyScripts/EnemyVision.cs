using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform origin;
    [SerializeField] private float viewRadius = 5;
    [SerializeField] private float viewAngle = 90;
    [SerializeField] private int nrOfRays = 8;
    [SerializeField] private LayerMask whatIsObstacle;
    [SerializeField] private LayerMask whatIsPlayer;

    void Update()
    {
        //RayCastVision();
        CircleVision();
        //transform.LookAt(player);
    }

    private void RayCastVision()
    {
        //search in circle around center
        float stepAngleDeg = viewAngle / nrOfRays; //angle between two sampled rays
        for (int i = 0; i < nrOfRays; i++)
        {
            Vector3 localRotation = new Vector3(i * stepAngleDeg- viewAngle/2, 0, 0); //direction of Ray to cast in Euler XYZ form
            Vector3 direction = (Quaternion.Euler(localRotation) * transform.forward).normalized; //turn rotation into direction
            //Vector3 direction = localRotation + transform.forward;
            

            Ray ray = new Ray(origin.position, direction);
            Debug.DrawLine(ray.origin, ray.origin + (direction * viewRadius), Color.red);
        }
    }

    private void CircleVision()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius);
        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirTOTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirTOTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);
                if(Physics.Raycast(transform.position, dirTOTarget, distToTarget, whatIsPlayer))
                    Debug.DrawLine(transform.position, dirTOTarget*distToTarget , Color.green);
                else
                    Debug.DrawLine(transform.position, dirTOTarget * distToTarget, Color.red);

            }
            else
                Debug.DrawLine(transform.position, dirTOTarget * viewRadius, Color.gray);
        }
    }
}
