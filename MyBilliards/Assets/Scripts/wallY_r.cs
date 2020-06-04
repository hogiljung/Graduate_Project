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
            Reflect(collision);
            Rotate(collision);
        }
    }
    
    private void Reflect(Collision collision)
    {
        Vector3 incomingVector = collision.collider.GetComponent<Rigidbody>().velocity;
        speed = incomingVector.magnitude;
        incomingVector = incomingVector.normalized;

        // 충돌한 면의 법선 벡터
        Vector3 normalVector = collision.GetContact(0).normal;

        // 법선 벡터와 입사벡터을 이용하여 반사벡터를 알아낸다.
        Vector3 reflectVector = Vector3.Reflect(incomingVector, normalVector); //반사각
        reflectVector = reflectVector.normalized;

        collision.collider.GetComponent<Rigidbody>().velocity = reflectVector * speed;
    }

    private void Rotate(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        speed = rb.velocity.magnitude * 0.1f;
        rb.AddForce(0, 0, -Mathf.Clamp(rb.angularVelocity.y * 0.96f, -30, 30) * speed);
        rb.velocity *= 1f - (Mathf.Clamp(speed * 0.5f, 0.12f, 0.32f));
    }
}
