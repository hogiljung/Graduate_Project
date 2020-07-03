using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManage : MonoBehaviour
{
    public AudioClip shotWeak;
    public AudioClip shotStrong;
    public AudioClip ballstrong;
    public AudioClip balleweak;
    public AudioClip wallCollide;
    public AudioClip menuOn;
    public AudioClip menuOff;
    public AudioClip button;
    
    public static SoundManage instance;
    
    //생성시 실행
    private void Awake()
    {
        if (SoundManage.instance == null)
            SoundManage.instance = this;

    }
}
