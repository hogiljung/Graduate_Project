using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallX : MonoBehaviour
{
    private float speed;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("ball"))
        {
            //Reflect(collision);
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

        collision.collider.GetComponent<Rigidbody>().velocity = reflectVector * speed * 0.68f;
    }
    
    private void Rotate(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        speed = rb.velocity.magnitude * 0.24f;
        if (speed > 1)
            speed = 1;
        rb.AddForce(Mathf.Clamp(rb.angularVelocity.y, -10f, 10f) * speed, 0, 0);
        rb.velocity *= 0.92f;
    }
}
