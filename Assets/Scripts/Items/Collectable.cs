using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Item
{
    public ParticleSystem glowFX;
    [SerializeField] float minRotationSpeed;
    [SerializeField] float maxRotationSpeed;
    protected float rotationSpeed;
    [SerializeField] float heightFromCenter;
    [SerializeField] float minFloatSpeed;
    [SerializeField] float maxFloatSpeed;
    protected float floatSpeed;
    private float orginY;
    private bool floatUp;
    protected bool hasTarget;
    Vector3 targetPos;
    Rigidbody rb;
    private float pullForce;

    protected virtual void Start()
    {
        floatUp = true;
        orginY = transform.position.y;
        //FixYPosition();
        glowFX.Play();
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        floatSpeed = Random.Range(minFloatSpeed, maxFloatSpeed);
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        //Float();
        Spin();
    }

    private void FixedUpdate()
    {
        SetVelocity();
    }

    protected virtual void Spin()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    protected void Float()
    {
        if (floatUp)
            FloatUp();
        else
            FloatDown();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            Debug.DrawLine(transform.position, hit.point);

        }
    }

    private void FloatUp()
    {
        transform.position += new Vector3(0, floatSpeed * Time.deltaTime, 0);
        if (transform.position.y >= orginY + heightFromCenter)
            floatUp = false;
    }

    private void FloatDown()
    {
        transform.position -= new Vector3(0, floatSpeed * Time.deltaTime, 0);
        if (transform.position.y <= orginY - heightFromCenter)
            floatUp = true;
    }
    protected virtual void PickUp()
    {

    }

    protected virtual void FixYPosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + orginY, transform.position.z);
        }
    }

    public void SetTarget(Vector3 pos, float strength)
    {
        targetPos = pos;
        hasTarget = true;
        pullForce = strength;
    }

    public void RemoveTarget()
    {
        targetPos = Vector3.zero;
        hasTarget = false;
        rb.velocity = Vector3.zero;
    }

    protected void SetVelocity()
    {
        if (hasTarget)
        {
            Vector3 targetposOffset = new Vector3(targetPos.x, targetPos.y + 1, targetPos.z);
            Vector3 targetDir = (targetposOffset - transform.position).normalized;
            rb.velocity = new Vector3(targetDir.x, targetDir.y, targetDir.z) * pullForce;
        }
    }

}
