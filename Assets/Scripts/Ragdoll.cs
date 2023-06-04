using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        setRigidBodyState(true);
        setColliderState(false);
        animator = GetComponent<Animator>();
    }

    void setRigidBodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            if (rigidbody.gameObject.CompareTag("Gun") || rigidbody.gameObject.CompareTag("Sword"))
                continue;

            rigidbody.isKinematic = state;
        }
    }

    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Gun") || collider.gameObject.CompareTag("Sword") || 
                collider.gameObject.CompareTag("Magnet"))
            {
                collider.enabled = true;
                continue; 
            }
               
            collider.enabled = state; 
        }

        colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
            collider.enabled = !state;
    }

    private void AddForce(float explosionForce, Vector3 forceLocation, float explosionRadius)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider closeObjects in colliders)
        {
            Rigidbody rigidbody = closeObjects.GetComponent<Rigidbody>();
            if (rigidbody != null)
                rigidbody.AddExplosionForce(explosionForce, forceLocation, explosionRadius);
        }
    }

    public void ActivateRagdollWithForce(float explosionForce, Vector3 forceLocation, float explosionRadius)
    {
        ActivateRagdoll();
        AddForce(explosionForce, forceLocation, explosionRadius);
    }
    public void ActivateRagdoll()
    {
        animator.enabled = false;
        if (TryGetComponent<PlayerAnimation>(out PlayerAnimation component))
            component.enabled = false;

        setRigidBodyState(false);
        setColliderState(true);
    }

}
