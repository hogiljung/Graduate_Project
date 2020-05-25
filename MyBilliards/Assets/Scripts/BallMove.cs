using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    Rigidbody rb;
    bool isMoving, isRotate;
    Vector3 velocity;
    Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isMoving = false;
        isRotate = false;
        velocity = new Vector3();
        rotation = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
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
        if (rb.velocity.magnitude < 3)
        {
            rb.velocity *= 0.9984f;
        }
        else if (rb.velocity.magnitude < 10)
        {
            rb.velocity *= 0.9989f;
        }
        else if (rb.velocity.magnitude < 15)
        {
            rb.velocity *= 0.9994f;
        }
    }

    private void CheckStop()
    {
        //속도 체크
        if (rb.velocity.magnitude < 0.001)
        {
            rb.velocity.Set(0, 0, 0);
        }
        else if (rb.velocity.magnitude < 0.01)
        {
            rb.velocity *= 0.8f;
        }

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
        SoundManage.instance.PlaySoundShot("ballCOllide");
    }
}

