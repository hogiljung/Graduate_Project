using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Swing : MonoBehaviour
{
    // 공 움직임 관련 스크립트
    private CueGrap cueGrap;
    public Transform ptrf;
    public SaveData savedata;
    public GameObject shadowBall;
    private Rigidbody colrb;
    private RaycastHit hit;
    private RaycastHit hit2;
    private RaycastHit hit3;
    

    private LineRenderer layser;        // 레이저
    private Vector3 rayDir;
    private Vector3 predictPos;
    private Vector3 prePos;
    private Vector3 velocity;
    private bool trigger;

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
        cueGrap = FindObjectOfType<CueGrap>();
        //layser = this.gameObject.AddComponent<LineRenderer>();
        layser = gameObject.GetComponent<LineRenderer>();
        layser.enabled = false;
        trigger = false;
        // 레이저 굵기 표현
        layser.startWidth = 0.002f;
        layser.endWidth = 0.002f;
    }

    //물리연산      (프로젝트 설정으로)0.02초마다 연산
    //*업데이트는 물리업데이트 사이사이에 존재해야 자연스러움
    private void FixedUpdate()
    {

    }
    //업데이트
    private void Update()
    {
        SetForce();

        if(layser.enabled)
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
                if (Physics.SphereCast(hit2.collider.transform.position, 0.0308f, rayDir, out hit3, 2f))
                {
                    predictPos = hit2.collider.transform.position + rayDir * hit3.distance;
                    if (!shadowBall.activeSelf)
                        shadowBall.SetActive(true);
                    shadowBall.transform.position = predictPos;
                }
                
            }
            else
            {
                //layser.SetPosition(1, transform.position);
                if(layser.enabled)
                    layser.enabled = false;
                if (shadowBall.activeSelf)
                    shadowBall.SetActive(false);
            }

        }
        else
        {
            //layser.SetPosition(1, transform.position);
            if(layser.enabled)
                layser.enabled = false;
            if(shadowBall.activeSelf)
                shadowBall.SetActive(false);
        }
    }

    private void SetForce()
    {
        if (cueGrap.IsGrap)
        {
            if (!trigger)
            {
                trigger = true;
                StartCoroutine("GetPrePos");
            }
        }
        else
        {
            if (trigger)
            {
                trigger = false;
                StopCoroutine("GetPrePos");
            }
        }
    }

    IEnumerator GetPrePos()
    {
        WaitForSeconds wait = new WaitForSeconds(0.03f);

        while (true)
        {
            prePos = ptrf.localPosition;
            //Debug.Log("prePos update");
            yield return wait;
            velocity = (ptrf.localPosition - prePos);
        }
    }

    //감지
    private void OnTriggerEnter(Collider other)
    {
        //공일때
        if (other.tag.Equals("ball"))
        {
            //SetData();
            Force(other);
            SoundManage.instance.PlaySoundShot("shot");
        }
    }

    private void SetData()
    {
        SaveData.Info info = new SaveData.Info();
        info.ID = "user";
        info.ball1pos = ball1.position;
        info.ball1rot = ball1.rotation.eulerAngles;
        info.ball2pos = ball2.position;
        info.ball2rot = ball2.rotation.eulerAngles;
        info.ball3pos = ball3.position;
        info.ball3rot = ball3.rotation.eulerAngles;
        info.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        savedata.SetData(info);
    }

    private void Force(Collider other)
    {
        //Debug.Log("trp" + ptrf.localPosition + "prep" + prePos + "V" + velocity);
        colrb = other.gameObject.GetComponent<Rigidbody>();
        colrb.velocity.Set(0, 0, 0);
        colrb.angularVelocity.Set(0, 0, 0);
        Physics.Raycast(ptrf.position, ptrf.forward, out hit, 2f);
        colrb.AddForceAtPosition(velocity * 270, hit.point);
        colrb.AddTorque(-velocity * 270f);
    }

    //접촉중
    private void OnTriggerStay(Collider other)
    {
        //공일때
        if (other.tag.Equals("ball"))
        {
            colrb.AddForceAtPosition(velocity * 25, hit.point);
            colrb.AddTorque(-velocity);
        }
    }
    
}
