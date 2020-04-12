using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    Transform tr;
    Rigidbody rb;
    bool isMoving, isRotate;
    Vector3 velocity;
    Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        isMoving = false;
        isRotate = false;
        velocity = new Vector3();
        rotation = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.tag == "ball1")
        {
            /*
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y -= (float)9.8;
            }
            */
            if (Input.GetKeyDown(KeyCode.A))
            {
                velocity.x -= 2f;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                velocity.z -= 2f;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                velocity.x += 2f;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                velocity.z += 2f;
            }
            rb.velocity = velocity;
        }

        CheckStop();
        //CheckCollision();
    }
    

    private void FixedUpdate()
    {
        /*
        if(velocity.y > -9.8)
            velocity.y -= 0.16333f;   //중력
        */
        if (isMoving)
        {
            Debug.Log("_BallMove\n");
            Ballmv();
        }
        /*
        if (isRotate)
        {
            Debug.Log("_BallRotate\n");
            Ballrt();
        }
        */
        
    }

    private void CheckCollision()
    {
        if (!isMoving)
        {
            if (Mathf.Abs(rb.velocity.magnitude) >= 0.1)
            {
                isMoving = true;
            }
        }
        if (!isRotate)
        {
            if (Mathf.Abs(rb.rotation.x) >= 0.1 || Mathf.Abs(rb.rotation.y) >= 0.1 || Mathf.Abs(rb.rotation.z) >= 0.1)
            {
                isRotate = true;
            }
        }
    }

    private void Ballmv()
    {
        velocity.x *= 0.9944f;
        velocity.z *= 0.9944f;
        if(velocity.x > 0)
        {
            velocity.x -= 0.01f;
        }
        else if (velocity.x < 0)
        {
            velocity.x += 0.01f;
        }
        if(velocity.z > 0)
        {
            velocity.z -= 0.01f;
        }
        else if(velocity.z < 0)
        {
            velocity.z += 0.01f;
        }
        rb.velocity = velocity;
        
    }

    private void Ballrt()
    {
        rotation.x *= 0.931f;
        rotation.y *= 0.931f;
        rotation.z *= 0.931f;
        rb.angularVelocity = rotation;
    }

    private void CheckStop()
    {
        if (Mathf.Abs(rb.velocity.magnitude) < 0.1)
        {
            BallStopmv();
        }
        if (Mathf.Abs(rb.rotation.x) < 0.1 || Mathf.Abs(rb.rotation.y) < 0.1 || Mathf.Abs(rb.rotation.z) < 0.1)
        {
            BallStoprt();
        }
    }

    private void BallStopmv()
    {
        velocity.x = 0;
        velocity.y = 0;
        velocity.z = 0;
        isMoving = false;
    }

    private void BallStoprt()
    {
        rotation.x = 0;
        rotation.y = 0;
        rotation.z = 0;
        isRotate = false;
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint cp = collision.GetContact(0);
        Vector3 dir = tr.position - cp.point;
        rb.AddForce((dir).normalized * velocity.magnitude);

    }
    */
    /*
    private void OnTriggerEnter(Collider other)
    {
        isMoving = true;
        isRotate = true;
        Vector3 temp;
        switch (rb.tag)
        {
            case "ball1":
                switch (other.tag)
                {
                    case "ball2":
                    case "ball3":
                        Rigidbody target = other.GetComponent<Rigidbody>();
                        temp = rb.velocity;
                        rb.velocity = target.velocity;
                        target.velocity = temp;
                        break;
                    case "wall":
                        temp = rb.velocity;
                        temp.x *= 0.65f * (-temp.x);
                        temp.z *= 0.65f * (-temp.z);
                        rb.velocity = temp;
                        break;
                    case "plain":
                        temp = rb.velocity;
                        temp.y *= 0.36f * (-temp.y);
                        rb.velocity = temp;
                        break;
                }
                break;
            case "ball2":
                switch (other.tag)
                {
                    case "ball3":
                        Rigidbody target = other.GetComponent<Rigidbody>();
                        temp = rb.velocity;
                        rb.velocity = target.velocity;
                        target.velocity = temp;
                        break;
                    case "wall":
                        temp = rb.velocity;
                        temp.x *= 0.65f * (-temp.x);
                        temp.z *= 0.65f * (-temp.z);
                        rb.velocity = temp;
                        break;
                    case "plain":
                        temp = rb.velocity;
                        temp.y *= 0.36f * (-temp.y);
                        rb.velocity = temp;
                        break;
                }
                break;
            case "ball3":
                switch (other.tag)
                {
                    case "wall":
                        temp = rb.velocity;
                        temp.x *= 0.65f * (-temp.x);
                        temp.z *= 0.65f * (-temp.z);
                        rb.velocity = temp;
                        break;
                    case "plain":
                        temp = rb.velocity;
                        temp.y *= 0.36f * (-temp.y);
                        rb.velocity = temp;
                        break;
                }
                break;
        }
        
    }
    */
    private void OnCollisionEnter(Collision collision)
    {
        velocity = rb.velocity;
    }
}

