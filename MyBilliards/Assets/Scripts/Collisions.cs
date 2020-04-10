using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    public GameObject ball1, ball2, ball3, wall1, wall2, wall3, wall4, plain;
    Rigidbody b1, b2, b3;
    // Start is called before the first frame update
    void Start()
    {
        b1 = ball1.GetComponent<Rigidbody>();
        b2 = ball2.GetComponent<Rigidbody>();
        b3 = ball3.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}
