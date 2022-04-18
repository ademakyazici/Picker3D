using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : LevelObject
{
    protected Rigidbody rb;


    public virtual void Start()
    {

    }
    public virtual void Release()
    {
        Vector3 forceVector = new Vector3(0, GameManager.Instance.CollectibleReleaseForce, GameManager.Instance.CollectibleReleaseForce);
        rb.AddForce(forceVector);
    }

    private void OnEnable()
    {
        EventManager.GameStarted += AddComponents;
    }

    private void OnDisable()
    {
        EventManager.GameStarted -= AddComponents;
    }

    private void AddComponents()
    {
        if (this.gameObject.GetComponent<Rigidbody>() == null)
            this.gameObject.AddComponent<Rigidbody>();

        rb = GetComponent<Rigidbody>();
        rb.mass = 0.5f;
        rb.angularDrag = 0;
        rb.drag = 0;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.useGravity = true;
    }

}
