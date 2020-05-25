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

    public AudioSource BGM;
    public AudioSource SFX;
    public AudioSource UI;
    public static SoundManage instance;
    
    //생성시 실행
    private void Awake()
    {
        if (SoundManage.instance == null)
            SoundManage.instance = this;
    }
    
    public void PlaySoundShot(string name)
    {
        
        switch (name)
        {
            case "shot":
                SFX.PlayOneShot(shot);
                break;
            case "ballCollide":
                SFX.PlayOneShot(ballCollide);
                break;
            case "wallCollide":
                SFX.PlayOneShot(ballCollide);
                break;
            case "MenuOn":
                UI.PlayOneShot(menuOn);
                break;
            case "MenuOff":
                UI.PlayOneShot(menuOff);
                break;
            case "button":
                UI.PlayOneShot(button);
                break;
        }
    }
}
