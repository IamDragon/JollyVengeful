using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
    public float weight = 1.0f;
}


public class WeaponIK : MonoBehaviour
{
    [SerializeField]  Transform targetTransform;
    [SerializeField] Transform aimTransform;
    [SerializeField] Transform headRayTransform;

    [SerializeField] int iterations = 10;
    [Range(0, 1)]
    [SerializeField] float weight = 1.0f;

    [SerializeField] HumanBone[] humanBones;
    [SerializeField] float angleLimit = 90;
    public float AngleLimit
    {
        get => angleLimit;
    }

    [SerializeField] float rotationSpeed = 1f;

    private Quaternion lookRotation;

    [SerializeField] float distanceLimit = 1.5f;
    Transform[] boneTransforms;

    void Start()
    {
        Animator animator = GetComponent<Animator>();
        boneTransforms = new Transform[humanBones.Length];
        for (int i = 0; i < boneTransforms.Length; i++)
        {
            boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone); 
        }
    }

    private void LateUpdate()
    {
        if (aimTransform != null && targetTransform != null)
        {
            Vector3 targetPosition = GetTargetPosition();
            for (int i = 0; i < iterations; i++)
            {
                for (int b = 0; b < boneTransforms.Length; b++)
                {
                    Transform bone = boneTransforms[b];
                    float boneWeight = humanBones[b].weight * weight;
                    AimAtTarget(bone, targetPosition, boneWeight);
                }
            }
        }
    }

    /// <summary>
    /// Calculate position and direction oftarget and returns the aimTransforms position with the direction
    /// </summary>
    /// <returns></returns>
    Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = targetTransform.position - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        lookRotation = Quaternion.LookRotation(targetDirection.normalized);
        float blendOut = 0.0f;

        //targetAngle är onödig
        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        float targetDistance = targetDirection.magnitude;

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        if (targetDistance < distanceLimit)
        {
            blendOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;
    }


    /// <summary>
    /// Set bone rotation to look towards target position
    /// </summary>
    /// <param name="bone"></param>
    /// <param name="targetposition"></param>
    /// <param name="weight"></param>
    private void AimAtTarget(Transform bone, Vector3 targetposition, float weight)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetposition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim)
    {
        if (aim == null)
        {
            aimTransform = headRayTransform;
        }
        else
        {
            aimTransform = aim;
        }
    }

   
}
