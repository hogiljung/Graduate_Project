using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    Rigidbody rb;
    Vector3 ang;
    private AudioSource audios;
    private bool shotsound;
    private bool ballsound;
    private bool wallsound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audios = GetComponent<AudioSource>();
        shotsound = true;
        ballsound = true;
        wallsound = true;
    }
    

    private void FixedUpdate()
    {
        Friction();     //저항
        CheckStop();    //멈춤상태 보정
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

    private void CheckStop()
    {
        //속도 체크
        if (rb.velocity.magnitude < 0.001)
            rb.velocity.Set(0, 0, 0);

        //회전 체크
        if (Mathf.Abs(rb.angularVelocity.x) < 0.1)
        {
            rb.angularVelocity.Set(0, rb.angularVelocity.y, rb.angularVelocity.z);
        }
        else if (Mathf.Abs(rb.angularVelocity.x) < 3)
        {
            rb.angularVelocity.Set(rb.angularVelocity.x * 0.8f, rb.angularVelocity.y, rb.angularVelocity.z);
        }

        if (Mathf.Abs(rb.angularVelocity.y) < 0.1)
        {
            rb.angularVelocity.Set(rb.angularVelocity.x, 0, rb.angularVelocity.z);
        }
        else if (Mathf.Abs(rb.angularVelocity.y) < 3)
        {
            rb.angularVelocity.Set(rb.angularVelocity.x, rb.angularVelocity.y * 0.8f, rb.angularVelocity.z);
        }

        if (Mathf.Abs(rb.angularVelocity.z) < 0.1)
        {
            rb.angularVelocity.Set(rb.angularVelocity.x, rb.angularVelocity.y, 0);
        }
        else if (Mathf.Abs(rb.angularVelocity.z) < 3)
        {
            rb.angularVelocity.Set(rb.angularVelocity.x, rb.angularVelocity.y, rb.angularVelocity.z * 0.8f);
        }
    }

    //사운드 쿨타임 & 사운드 판정
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

    IEnumerator WallSound()
    {
        wallsound = false;
        audios.PlayOneShot(SoundManage.instance.wallCollide);

        yield return new WaitForSeconds(0.05f);
        wallsound = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("ball"))
        {
            if (ballsound)
                StartCoroutine(BallSound());
        }

        else if (collision.gameObject.tag.Equals("wall"))
        {
            if (wallsound)
                StartCoroutine(WallSound());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("cue"))
        {
            if (shotsound)
            {
                shotsound = false;
                StartCoroutine(ShotSound());
            }
        }
    }

}

