using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    Rigidbody rb;
    float test;
    Vector3 ang;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        test = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("wx: " + test*1000000);
        /*
        //키보드로 공 움직이기 WASD
        if (Input.GetKeyDown(KeyCode.A))
        {
            velocity.x = -2f;
            rb.velocity = velocity;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            velocity.z = -2f;
            rb.velocity = velocity;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            velocity.x = +2f;
            rb.velocity = velocity;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            velocity.z = +2f;
            rb.velocity = velocity;
        }
        */
    }

    private void FixedUpdate()
    {
        Friction();     //저항
        CheckStop();    //멈춤상태 보정
    }

    private void Friction()
    {
        float magnitude = rb.velocity.magnitude;
        //속도 저항
        if (magnitude == 0)
        {
            rb.angularVelocity *= 0.9974f;
        }
        else if (magnitude < 0.05)
        {
            rb.velocity *= 0.989f;
            rb.angularVelocity *= 0.9974f;
        }
        else if (magnitude < 3)
        {
            rb.velocity *= 0.9979f;
            rb.angularVelocity *= 0.9979f;
        }
        else if (magnitude < 10)
        {
            rb.velocity *= 0.9984f;
            rb.angularVelocity *= 0.9984f;
        }
        else if (magnitude < 15)
        {
            rb.velocity *= 0.9989f;
            rb.angularVelocity *= 0.9989f;
        }
    }

    private void CheckStop()
    {
        //속도 체크
        if (rb.velocity.magnitude < 0.001)
            rb.velocity.Set(0, 0, 0);

        //회전 체크
        if (Mathf.Abs(rb.angularVelocity.x) < 0.1)
        {
            rb.angularVelocity.Set(0, rb.angularVelocity.y, rb.angularVelocity.z);
        }
        else if (Mathf.Abs(rb.angularVelocity.x) < 3)
        {
            rb.angularVelocity.Set(rb.angularVelocity.x * 0.8f, rb.angularVelocity.y, rb.angularVelocity.z);
        }

        if (Mathf.Abs(rb.angularVelocity.y) < 0.1)
        {
            rb.angularVelocity.Set(rb.angularVelocity.x, 0, rb.angularVelocity.z);
        }
        else if (Mathf.Abs(rb.angularVelocity.y) < 3)
        {
            rb.angularVelocity.Set(rb.angularVelocity.x, rb.angularVelocity.y * 0.8f, rb.angularVelocity.z);
        }

        if (Mathf.Abs(rb.angularVelocity.z) < 0.1)
        {
            rb.angularVelocity.Set(rb.angularVelocity.x, rb.angularVelocity.y, 0);
        }
        else if (Mathf.Abs(rb.angularVelocity.z) < 3)
        {
            rb.angularVelocity.Set(rb.angularVelocity.x, rb.angularVelocity.y, rb.angularVelocity.z * 0.8f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag.Equals("ball"))
            SoundManage.instance.PlaySoundShot("ballCOllide");
        else if(collision.collider.tag.Equals("wall"))
            SoundManage.instance.PlaySoundShot("wallCollide");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("ball"))
            SoundManage.instance.PlaySoundShot("ballCOllide");

    }
}

