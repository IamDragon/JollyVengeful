using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HandPlacementIK : MonoBehaviour
{
    [SerializeField] Rig animationRig;
    [SerializeField] Transform defaultTarget;
    [SerializeField] Transform defaultHint;
    Transform targetLocation;
    Transform hintLocation;
    TwoBoneIKConstraint secondHandConstraint;

    // Start is called before the first frame update
    void Start()
    {
        secondHandConstraint = animationRig.GetComponentInChildren<TwoBoneIKConstraint>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLocationAndRotation();

    }

    void UpdateLocationAndRotation()
    {
        if (hintLocation != null && targetLocation != null)
        {
            defaultHint.position = hintLocation.position;
            defaultTarget.position = targetLocation.position;
            defaultHint.rotation = hintLocation.rotation;
            defaultTarget.rotation = targetLocation.rotation;
        }
        
    }

    public void SetTargetLocation(Transform target)
    {
        targetLocation = target;
    }

    public void SetHintLocation(Transform hint)
    {
        hintLocation = hint;
    }

    public void ClearTargetAndHint()
    {
        targetLocation = null;
        hintLocation = null;
    }

    public void SetRigWeight(float weight)
    {
        animationRig.weight = weight;
    }

    public float GetRigWeight()
    {
        return animationRig.weight;
    }
}
