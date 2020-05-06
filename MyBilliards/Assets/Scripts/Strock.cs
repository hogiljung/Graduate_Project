using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class strock : MonoBehaviour
{
    public GameObject stick, ball;  //유니티에서 설정
    Rigidbody stick_rb;
    Rigidbody ball_rb;
    RaycastHit hit;
    float distance;
    bool isReady;

    // Start is called before the first frame update
    void Start()
    {
        stick_rb = stick.GetComponent<Rigidbody>();
        ball_rb = ball.GetComponent<Rigidbody>();
        isReady = false;
        distance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            Debug.Log("_Stroke\n");
            Stroke();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "ball")
        {
            Debug.Log("trigger stroke: stay\n");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "ball")
        {
            Debug.Log("trigger stroke: exit\n");
        }
    }

    void Stroke()
    {
        Vector3 stick_speed = stick_rb.velocity;

        distance = stick_rb.velocity.magnitude;

        Debug.DrawRay(transform.position, transform.forward * distance, Color.blue, 0.3f);
        if (Physics.Raycast(stick_rb.position, transform.forward, out hit, distance))
        {
            hit.rigidbody.velocity = stick_rb.velocity;
        }
    }

}
