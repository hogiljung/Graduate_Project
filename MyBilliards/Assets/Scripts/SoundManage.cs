using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManage : MonoBehaviour
{
    public AudioClip shot;
    public AudioClip ballCollide;
    public AudioClip menuOn;
    public AudioClip menuOff;
    public AudioClip button;

    AudioSource audioSource;
    public static SoundManage instance;

    //생성시 실행
    private void Awake()
    {
        if (SoundManage.instance == null)
            SoundManage.instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    
    public void PlaySoundShot(string name)
    {
        
        switch (name)
        {
            case "shot":
                audioSource.PlayOneShot(shot);
                break;
            case "ballCollide":
                audioSource.PlayOneShot(ballCollide);
                break;
            case "MenuOn":
                audioSource.PlayOneShot(menuOn);
                break;
            case "MenuOff":
                audioSource.PlayOneShot(menuOff);
                break;
            case "button":
                audioSource.PlayOneShot(button);
                break;
        }
    }
}
