using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallZ : MonoBehaviour
{
    Vector3 dir;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        dir = collision.collider.GetComponent<Rigidbody>().velocity;
        dir.z = -dir.z;
    }
    private void OnTriggerEnter(Collider other)
    {
        dir = other.GetComponent<Rigidbody>().velocity;
        dir.z = -dir.z;
    }
}
