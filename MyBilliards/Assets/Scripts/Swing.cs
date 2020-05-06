using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    // 공 움직임 관련 스크립트
    public Transform ptrf;
    private Rigidbody colrb;
    private RaycastHit hit;

    private Vector3 prePos;
    private Vector3 velocity;
    private bool trigger = false;
    private float touchTime = 0f;

    private int count;
    
    /*
    Unity 내부 함수 실행 순서
    Strat - FixedUpdate - 내부 물리 처리 - OnTrigger - OnCollision
    - Update - yield들 - 내부 애니메이션 업데이트 - LateUpdate
    - 화면 렌더 - gizmo 렌더 - UI 렌더 - 마무리
    */
    void Start()
    {
        StartCoroutine("GetPrePos");
        count = 0;
    }

    //물리연산      (프로젝트 설정으로)0.02초마다 연산
    //*업데이트는 물리업데이트 사이사이에 존재해야 자연스러움
    private void FixedUpdate()
    {

    }
    //업데이트
    private void Update()
    {

    }
    //업데이트 이후 업데이트
    private void LateUpdate()
    {
    }
    //그후 화면출력

    IEnumerator GetPrePos()
    {
        while (true)
        {
            prePos = ptrf.localPosition;
            yield return new WaitForSeconds(0.05f);
            velocity = (ptrf.localPosition - prePos);
        }
    }

    //감지
    private void OnTriggerEnter(Collider other)
    {
        //공일때
        if (other.CompareTag("ball"))
        {
            Debug.Log("trp" + ptrf.localPosition + "prep" + prePos + "V" + velocity);
            colrb = other.gameObject.GetComponent<Rigidbody>();
            Debug.DrawRay(ptrf.position, ptrf.forward * 10f, Color.yellow, 0.5f);
            Physics.Raycast(ptrf.position, ptrf.forward, out hit, 2f);
            colrb.AddForceAtPosition(velocity * 450, hit.point);
        }
    }

    //접촉중
    private void OnTriggerStay(Collider other)
    {
        //공일때
        if (other.CompareTag("ball"))
        {
            Debug.Log("ball stay " + count);
            count++;
            colrb.AddForceAtPosition(velocity, hit.point);
        }
    }

    //탈출
    private void OnTriggerExit(Collider other)
    {
        count = 0;
    }

    /*
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
    */
}
