﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallX_r : MonoBehaviour
{
    private float speed;
    private float e;       //쿠션 반발계수
    private float mu;      //쿠션 마찰계수

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("ball"))
        {
            Reflect(collision);
            //Rotate(collision);
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
        
        e = -Mathf.Pow(5f / 7f, speed + 3.12f) * 0.9f + 1f;
        mu = 0.471f - 0.241f * Vector3.Dot(rb.velocity, new Vector3(0, 0, rb.velocity.x));
        //Debug.Log("e" + e + " mu " + mu);
        //Debug.Log("speed: " + speed);

        rb.velocity = reflectVector * speed * e + new Vector3(-Mathf.Clamp(rb.angularVelocity.y * 0.023f, -10f, 10f), 0, 0) * Mathf.Clamp(speed / 2f, 0.2f, 1.74f);
        rb.angularVelocity = rb.angularVelocity * (1f - mu);
    }
}
