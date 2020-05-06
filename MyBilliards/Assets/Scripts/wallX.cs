using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallX : MonoBehaviour
{
    private float speed;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ball"))
        {
            Reflect(collision);
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
    

    
    
}
