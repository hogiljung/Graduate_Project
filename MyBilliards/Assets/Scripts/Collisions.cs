using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    bool trigger = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "cue")
        {
            trigger = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "plain")
        {
            //마찰력

            //순구름운동 여부 판단
            if (this.isTrueRotate()) { }

            else
            {
                //float velX = 
            }
        }
        if (tag == "ball")
        {

        }
        if (tag == "cushion")
        {

        }
    }

    private bool isTrueRotate()
    {
        float velocityX = this.GetComponent<Rigidbody>().velocity.x;
        float velocityZ = this.GetComponent<Rigidbody>().velocity.z;
        float angularVelX = this.GetComponent<Rigidbody>().angularVelocity.x;
        float angularVelZ = this.GetComponent<Rigidbody>().angularVelocity.z;

        float slideVelX = velocityX - angularVelZ;
        float slideVelZ = velocityZ + angularVelX;

        // 순구름운동
        if (slideVelX <= 0.0005 && slideVelZ <= 0.0005)
            Debug.Log("slideVelX: " + slideVelX + ", slideVelY: " + slideVelZ);
        // time = 2rw0 / 7ug
        float radius = this.GetComponent<SphereCollider>().radius;
        Vector3 firstAngularVel = this.GetComponent<Rigidbody>().angularVelocity;
        float coeffOfFriction = GameObject.FindGameObjectWithTag("plain")
            .GetComponent<MeshCollider>().material.dynamicFriction;
        Vector3 gravity = Physics.gravity;

        float time = (2 * radius * firstAngularVel.magnitude) /
            (7 * coeffOfFriction * gravity.magnitude);

        Debug.Log("순구름 운동 시작 시간:" + time);
        //if(slideVelX <= rowThreshold && slideVelY <= rowThreshold)
        //{
        //    return true;
        //}

        return false;
    }
}
