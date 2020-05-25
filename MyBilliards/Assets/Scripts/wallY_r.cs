using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallY_r : MonoBehaviour
{
    private float speed;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("ball"))
        {
            Rotate(collision);
        }
    }

    private void Rotate(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        speed = rb.velocity.magnitude * 0.1f;
        if (speed > 0.24f)
            speed = 0.24f;
        rb.AddForce(0, 0, -Mathf.Clamp(rb.angularVelocity.y, -10f, 10f) * speed);
        rb.velocity *= 0.92f;
    }
}
