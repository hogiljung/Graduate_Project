using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallY : MonoBehaviour
{
    private float speed;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Reflects(other);
    }

    private void Reflects(Collider other)
    {

        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        speed = rb.velocity.magnitude;
        rb.velocity = Vector3.Reflect(rb.velocity.normalized, -transform.forward) * speed * 0.7f;
    }
}
