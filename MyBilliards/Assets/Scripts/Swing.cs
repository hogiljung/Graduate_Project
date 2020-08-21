using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Valve.VR;

public class Swing : MonoBehaviour
{
    // 공 움직임 관련 스크립트
    //private CueGrap cueGrap;
    private ScoreManager score;
    public Transform ptrf;
    public GameObject shadowBall;
    public GameObject guidUI1;
    public GameObject guidUI2;
    private Rigidbody colrb;
    private RaycastHit hit;
    private RaycastHit hit2;
    private RaycastHit hit3;
    
    private LineRenderer layser;        // 레이저
    private Vector3 rayDir;
    private Vector3 predictPos;
    private Vector3 prePos;
    private Vector3 prePos2;
    private Vector3 velocity;
    private int limit;              // 가중힘 리미트
    //private bool trigger;
    public SteamVR_Action_Vibration haptic;

    //리플레이 데이터
    public Transform ball1;
    public Transform ball2;
    public Transform ball3;
    /*
    Unity 내부 함수 실행 순서
    Strat - FixedUpdate - 내부 물리 처리 - OnTrigger - OnCollision
    - Update - yield들 - 내부 애니메이션 업데이트 - LateUpdate
    - 화면 렌더 - gizmo 렌더 - UI 렌더 - 마무리
    */

    void Start()
    {
        //cueGrap = FindObjectOfType<CueGrap>();
        //layser = this.gameObject.AddComponent<LineRenderer>();
        score = FindObjectOfType<ScoreManager>();
        layser = gameObject.GetComponent<LineRenderer>();
        layser.enabled = false;
        //trigger = false;
        // 레이저 굵기 표현
        layser.startWidth = 0.005f;
        layser.endWidth = 0.005f;
    }
    
    //업데이트
    private void Update()
    {
        //SetForce();

        if (layser.enabled)
            layser.SetPosition(0, transform.position);
        if (Physics.Raycast(transform.position, transform.forward, out hit2, 2f))
        {
            if (hit2.collider.tag.Equals("ball"))
            {
                if (!layser.enabled)
                    layser.enabled = true;
                layser.SetPosition(1, hit2.point);
                rayDir.Set(transform.forward.x, 0f, transform.forward.z);
                rayDir.Normalize();
                if (Physics.SphereCast(hit2.collider.transform.position, 0.0308f, rayDir, out hit3, 3f))
                {
                    predictPos = hit2.collider.transform.position + rayDir * hit3.distance;
                    if (!shadowBall.activeSelf)
                        shadowBall.SetActive(true);
                    shadowBall.transform.position = predictPos;
                }

            }
            else
            {
                if (layser.enabled)
                    layser.enabled = false;
                if (shadowBall.activeSelf)
                    shadowBall.SetActive(false);
            }

        }
        else
        {
            if (layser.enabled)
                layser.enabled = false;
            if (shadowBall.activeSelf)
                shadowBall.SetActive(false);
        }
    }

    //프레임 갱신 전 마지막 업데이트때 큐의 이전위치를 저장한다.
    private void LateUpdate()
    {
        prePos2 = prePos;
        prePos = ptrf.localPosition;
    }
    /*
    private void SetForce()
    {
        
        
        if (cueGrap.IsGrap)
        {
            if (!trigger)
            {
                trigger = true;
                StartCoroutine(GetPrePos());
            }
        }
        else
        {
            if (trigger)
            {
                trigger = false;
                StopCoroutine(GetPrePos());
            }
        }
        
    }

    IEnumerator GetPrePos()
    {
        WaitForSeconds wait = new WaitForSeconds(0.01f);

        while (true)
        {
            prePos = ptrf.localPosition;
            //Debug.Log("prePos update");
            yield return wait;
            velocity = (ptrf.localPosition - prePos);
        }
    }
    */

    //감지
    private void OnTriggerEnter(Collider other)
    {
        //공일때
        if (other.tag.Equals("ball"))
        {
            Force(other);   //타격힘 계산
            if(guidUI1.activeSelf)
                guidUI1.SetActive(false);
            if (guidUI2.activeSelf)
                guidUI2.SetActive(false);
            //수구 지정
            if (other.gameObject.name.Equals("WhiteBall"))
            {
                score.ball = 0;
            }
            else if (other.gameObject.name.Equals("YellowBall"))
            {
                score.ball = 1;
            }
        }
    }

    private void Force(Collider other)
    {
        velocity = (ptrf.localPosition - prePos2) * Time.deltaTime;     //힘이 프레임이 아닌 시간 의존적이도록 델타타임 적용 
        float mg = velocity.magnitude;
        //Debug.Log("trp" + ptrf.localPosition + "prep" + prePos + "V" + velocity);
        colrb = other.gameObject.GetComponent<Rigidbody>();
        colrb.velocity.Set(0, 0, 0);
        colrb.angularVelocity.Set(0, 0, 0);
        Physics.Raycast(ptrf.position, ptrf.forward, out hit, 1f);
        colrb.AddForceAtPosition(transform.forward * mg * 42500f, hit.point);
        //colrb.AddTorque(transform.forward * mg);
        //타격 진동
        haptic.Execute(0, 0.05f, 200, mg * 120f, SteamVR_Input_Sources.RightHand);
        haptic.Execute(0, 0.05f, 200, mg * 50f, SteamVR_Input_Sources.LeftHand);
    }

    //접촉중 (밀어치기에 따른 힘의 차이)
    private void OnTriggerStay(Collider other)
    {
        //공일때
        if (other.tag.Equals("ball"))
        {
            colrb.AddForce(transform.forward * velocity.magnitude * 425f);
        }
    }
}
