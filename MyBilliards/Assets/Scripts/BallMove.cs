using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    public bool collor;
    Rigidbody rb;
    Vector3 ang;
    private AudioSource audios;
    private bool shotsound;
    private bool ballsound;
    private bool wallsound;
    private bool ball_set;
    private bool target1;
    private bool target2;

    private ScoreManager score;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audios = GetComponent<AudioSource>();
        score = FindObjectOfType<ScoreManager>();
        shotsound = true;
        ballsound = true;
        wallsound = true;
        ball_set = false;
        target1 = false;
        target2 = false;
    }

    

    private void FixedUpdate()
    {
        Friction();     //움직임 저항
        CheckStop();    //이동 멈춤 보정
        CheckRotation();    //회전 멈춤 보정
    }

    private void Friction()
    {
        float magnitude = rb.velocity.magnitude;
        //속도 저항
        if (magnitude == 0)
        {
            rb.angularVelocity *= 0.9974f;
        }
        else if (magnitude < 0.05)
        {
            rb.velocity *= 0.989f;
            rb.angularVelocity *= 0.9974f;
        }
        else if (magnitude < 1)
        {
            rb.velocity *= 0.9979f;
            rb.angularVelocity *= 0.9979f;
        }
        else if (magnitude < 8)
        {
            rb.velocity *= 0.9984f;
            rb.angularVelocity *= 0.9984f;
        }
        else if (magnitude < 15)
        {
            rb.velocity *= 0.9989f;
            rb.angularVelocity *= 0.9989f;
        }
        
    }

    //정지 체크
    private void CheckStop()
    {
        if (rb.velocity.magnitude < 0.001f && rb.velocity.magnitude > 0.0f)
        {
            rb.velocity.Set(0, 0, 0);   //정지

            score.isEnd++;
            Debug.Log("isEnd = "+ score.isEnd);
            if (score.isEnd == 3)    //공이 모두 멈추면 스코어세팅 초기화
            {
                score.TurnEnd();
            }
        }

        
    }

    //회전 멈춤 보정
    private void CheckRotation()
    {
        float x = Mathf.Abs(rb.angularVelocity.x);
        float y = Mathf.Abs(rb.angularVelocity.y);
        float z = Mathf.Abs(rb.angularVelocity.z);
        float x2 = 1, y2 = 1, z2 = 1;
        bool changed = false;

        if (x < 0.001f)
        {
            
        }
        else if (x < 0.1f)
        {
            x2 = 0;
            changed = true;
            //rb.angularVelocity.Set(0, rb.angularVelocity.y, rb.angularVelocity.z);
        }
        else if (x < 1)
        {
            x2 = 0.6f;
            changed = true;
            //rb.angularVelocity.Set(rb.angularVelocity.x * 0.6f, rb.angularVelocity.y, rb.angularVelocity.z);
        }
        else if (x < 3)
        {
            x2 = 0.8f;
            changed = true;
            //rb.angularVelocity.Set(rb.angularVelocity.x * 0.8f, rb.angularVelocity.y, rb.angularVelocity.z);
        }

        if (y < 0.001f)
        {

        }
        else if (y < 0.1f)
        {
            y2 = 0;
            changed = true;
            //rb.angularVelocity.Set(rb.angularVelocity.x, 0, rb.angularVelocity.z);
        }
        else if (y < 1)
        {
            y2 = 0.6f;
            changed = true;
            //rb.angularVelocity.Set(rb.angularVelocity.x, rb.angularVelocity.y * 0.6f, rb.angularVelocity.z);
        }
        else if (y < 3)
        {
            y2 = 0.8f;
            changed = true;
            //rb.angularVelocity.Set(rb.angularVelocity.x, rb.angularVelocity.y * 0.8f, rb.angularVelocity.z);
        }

        if (z < 0.001f)
        {

        }
        else if (z < 0.1f)
        {
            z2 = 0;
            changed = true;
            //rb.angularVelocity.Set(rb.angularVelocity.x, rb.angularVelocity.y, 0);
        }
        else if (z < 1)
        {
            z2 = 0.6f;
            changed = true;
            //rb.angularVelocity.Set(rb.angularVelocity.x, rb.angularVelocity.y, rb.angularVelocity.z * 0.6f);
        }
        else if (z < 3)
        {
            z2 = 0.8f;
            changed = true;
            //rb.angularVelocity.Set(rb.angularVelocity.x, rb.angularVelocity.y, rb.angularVelocity.z * 0.8f);
        }
        //변경사항 있을때 적용
        if (changed)
            rb.angularVelocity.Set(rb.angularVelocity.x * x2, rb.angularVelocity.y * y2, rb.angularVelocity.z * z2);
    }

    //사운드 쿨타임 & 사운드 재생
    //타격음
    IEnumerator ShotSound()
    {
        Debug.Log("ballspd: " + rb.velocity.magnitude);
        
        if (rb.velocity.magnitude > 1f)
            audios.PlayOneShot(SoundManage.instance.shotStrong);
        else
            audios.PlayOneShot(SoundManage.instance.shotWeak);

        yield return new WaitForSeconds(0.5f);
        shotsound = true;
       
    }
    
    //공 충돌음
    IEnumerator BallSound()
    {
        ballsound = false;
        if(rb.velocity.magnitude > 1)
            audios.PlayOneShot(SoundManage.instance.ballstrong);
        else
            audios.PlayOneShot(SoundManage.instance.balleweak);

        yield return new WaitForSeconds(0.05f);
        ballsound = true;
    }

    //벽 충돌음
    IEnumerator WallSound()
    {
        wallsound = false;
        audios.PlayOneShot(SoundManage.instance.wallCollide);

        yield return new WaitForSeconds(0.05f);
        wallsound = true;
    }

    //충돌 발생시
    private void OnCollisionEnter(Collision collision)
    {
        //공 충돌일때
        if (collision.gameObject.tag.Equals("ball"))
        {
            if (ballsound)
                StartCoroutine(BallSound());
            if (ball_set)     //수구가 이 공이고
            {
                if (!score.success)     //3쿠션 성공이 아직 아닐때
                {
                    if (collision.gameObject.name.Equals("RedBall"))   //타격한 공 체크
                    {
                        target2 = true;
                        Debug.Log("target2");
                    }
                    else
                    {
                        target1 = true;
                        Debug.Log("target1");
                    }

                    if (score.cusion > 2 && target1 && target2)        //3쿠션 성공
                    {
                        Success3Cusion();
                    }
                }
            }
        }

        //쿠션 충돌일때
        else if (collision.gameObject.tag.Equals("wall"))
        {
            if (wallsound)
                StartCoroutine(WallSound());
            if (ball_set)       //수구가 이 공이면
            {
                score.cusion++;     //쿠션 카운트 증가
                Debug.Log("cusion : " + score.cusion);
            }
        }
    }

    //3쿠션 성공시 점수 증가, 성공 상태로 변경
    private void Success3Cusion()
    {
        score.success = true;
        score.score++;
        ball_set = false;
        Debug.Log("Score : " + score.score);
        score.ball = -1;
    }

    //큐가 타격되면
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("cue"))
        {
            if (shotsound)
            {
                shotsound = false;
                StartCoroutine(ShotSound());
                ball_set = true;        //이 공을 수구로 지정
            }
        }
    }

}

