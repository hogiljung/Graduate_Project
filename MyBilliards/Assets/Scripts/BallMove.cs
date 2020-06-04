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
        Debug.Log("wx: " + test*1000000);
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
        //CheckStop();    //멈춤상태 보정
    }

    private void Friction()
    {
        //속도 저항
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

        //회전 저항
        test = (-5 / 7 / (0.03075f * 0.03075f * rb.mass) * rb.angularVelocity.x / (Mathf.Sqrt(rb.angularVelocity.x * rb.angularVelocity.x + rb.angularVelocity.y * rb.angularVelocity.y)));
        ang.Set(-5.0f / 7.0f * 0.00038f / (0.03075f * 0.03075f * rb.mass) * rb.angularVelocity.x / (Mathf.Sqrt(rb.angularVelocity.x * rb.angularVelocity.x + rb.angularVelocity.y * rb.angularVelocity.y)),
            -5.0f / 2.0f * 0.000384f / (0.03075f * 0.03075f * rb.mass) * rb.angularVelocity.z / (Mathf.Sqrt(rb.angularVelocity.x * rb.angularVelocity.x + rb.angularVelocity.y * rb.angularVelocity.y)), 
            -5.0f / 7.0f * 0.00038f / (0.03075f * 0.03075f * rb.mass) * rb.angularVelocity.y / (Mathf.Sqrt(rb.angularVelocity.x * rb.angularVelocity.x + rb.angularVelocity.y * rb.angularVelocity.y)));
        //Debug.Log("wx: " + -5.0f / 7.0f * 0.00038f / (0.03075f * 0.03075f * rb.mass) * rb.angularVelocity.x / (Mathf.Sqrt(rb.angularVelocity.x * rb.angularVelocity.x + rb.angularVelocity.y * rb.angularVelocity.y)));
        // Debug.Log("wy: " + -5.0f / 7.0f * 0.00038f / (0.03075f * 0.03075f * rb.mass) * rb.angularVelocity.y / (Mathf.Sqrt(rb.angularVelocity.x * rb.angularVelocity.x + rb.angularVelocity.y * rb.angularVelocity.y)));
        // Debug.Log("wz: " + -5.0f / 2.0f * 0.000384f / (0.03075f * 0.03075f * rb.mass) * rb.angularVelocity.z / (Mathf.Sqrt(rb.angularVelocity.x * rb.angularVelocity.x + rb.angularVelocity.y * rb.angularVelocity.y)));
        Debug.Log("vx:" + -5.0f / 7.0f * 0.00038f / (0.03075f * 0.03075f * rb.mass) * rb.angularVelocity.y / (Mathf.Sqrt(rb.angularVelocity.x * rb.angularVelocity.x + rb.angularVelocity.y * rb.angularVelocity.y)));
        if (rb.velocity.magnitude > 0)
        {
            //rb.angularVelocity += ang;
            //rb.transform.rotation.eulerAngles.Set(-5.0f / 7.0f * 0.00038f / (0.03075f * 0.03075f * rb.mass) * rb.angularVelocity.x / (Mathf.Sqrt(rb.angularVelocity.x * rb.angularVelocity.x + rb.angularVelocity.y * rb.angularVelocity.y)) * Time.fixedDeltaTime, 
            //    -5.0f / 2.0f * 0.000384f / (0.03075f * 0.03075f * rb.mass) * rb.angularVelocity.z / (Mathf.Sqrt(rb.angularVelocity.x * rb.angularVelocity.x + rb.angularVelocity.y * rb.angularVelocity.y)), 
            //    -5.0f / 7.0f * 0.00038f / (0.03075f * 0.03075f * rb.mass) * rb.angularVelocity.y / (Mathf.Sqrt(rb.angularVelocity.x * rb.angularVelocity.x + rb.angularVelocity.y * rb.angularVelocity.y)));
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

