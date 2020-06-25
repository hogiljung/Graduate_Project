using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grounds : MonoBehaviour
{

    private float speed;

    private void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("ball"))
        {
            Reflect(collision);
        }
    }

    private void Reflect(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        Vector3 incomingVector = rb.velocity;
        speed = incomingVector.magnitude;
        incomingVector = incomingVector.normalized;

        // 충돌한 면의 법선 벡터
        Vector3 normalVector = collision.GetContact(0).normal;

        // 법선 벡터와 입사벡터을 이용하여 반사벡터를 알아낸다.
        Vector3 reflectVector = Vector3.Reflect(incomingVector, normalVector); //반사각
        reflectVector = reflectVector.normalized;

        rb.velocity = reflectVector * speed * 0.65f;
    }
}
