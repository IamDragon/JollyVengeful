using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootPlacement : MonoBehaviour
{
    [SerializeField] float rotationWeight = 0.5f;
    [SerializeField] LayerMask layerMask;

    [Range (0, 1f)]
    [SerializeField] float DistanceToGround;

    Animator anim;

    
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("IKLeftFootWeight"));
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, rotationWeight); 
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, anim.GetFloat("IKRightFootWeight"));
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, rotationWeight);


            // Left Foot
            RaycastHit hit;

            Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask))
            {
                if (hit.transform.tag == "Walkable")
                {
                    Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);

                    //Debug.Log("Walkable hit");
                    Vector3 footposition = hit.point;
                    footposition.y += DistanceToGround;
                    anim.SetIKPosition(AvatarIKGoal.LeftFoot, footposition);
                    anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(forward, hit.normal));
                }
            }

            // Right foot

            ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask))
            {
                if (hit.transform.tag == "Walkable")
                {
                    Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);

                    //Debug.Log("Walkable hit");
                    Vector3 footposition = hit.point;
                    footposition.y += DistanceToGround;
                    anim.SetIKPosition(AvatarIKGoal.RightFoot, footposition);
                    anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(forward, hit.normal));
                }
            }

        }
    }
}
