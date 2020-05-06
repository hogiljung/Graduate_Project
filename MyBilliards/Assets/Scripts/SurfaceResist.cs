using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    public GameObject ball;
    public GameObject surface;
    private Rigidbody rb_ball;
    // Start is called before the first frame update
    void Start()
    {
        rb_ball = ball.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float speed = Mathf.Sqrt(Mathf.Pow(rb_ball.velocity.x, 2) + Mathf.Pow(rb_ball.velocity.y, 2));
        Vector3 dir = rb_ball.velocity.normalized;
        Vector3 angular_speed = rb_ball.angularVelocity;
        
    }
}
