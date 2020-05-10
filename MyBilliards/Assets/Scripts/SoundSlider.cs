using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Audio;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public AudioMixerEffectPlugin bgm;
    public Slider slider;
    
    public void BGMControl()
    {
        float sound = slider.value;

        if (sound == -40f)
            bgm.SetFloatParameter("BGM", -80f);
        else
            bgm.SetFloatParameter("BGM", sound);
    }
}
