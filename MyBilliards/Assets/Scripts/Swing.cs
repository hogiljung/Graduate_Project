using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    // 공 움직임 관련 스크립트

    // 수구와 큐의 충돌 체크
    bool trigger = false;
    float touchTime = 0f;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    private void FixedUpdate()
    {

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ball2")
        {
            Debug.Log(collision.gameObject + "Enter");
            trigger = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "ball2")
        {
            Debug.Log(collision.gameObject + "Stay");
            touchTime += 0.1f;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ball2")
        {
            Debug.Log(touchTime);
            Debug.Log(collision.gameObject + "Exit");
            trigger = false;
            touchTime = 0f;
        }
    }
}
