using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    private void Start()
    {
        
    }

    public void MaterControl()
    {
        float sound = slider.value;

        if (sound == -40f)
            mixer.SetFloat("Master", -80f);
        else
            mixer.SetFloat("Master", sound);
    }

    public void BGMControl()
    {
        float sound = slider.value;

        if (sound == -40f)
            mixer.SetFloat("BGM", -80f);
        else
            mixer.SetFloat("BGM", sound);
    }

    public void SFXComtrol()
    {
        float sound = slider.value;

        if (sound == -40f)
            mixer.SetFloat("SFX", -80f);
        else
            mixer.SetFloat("SFX", sound);
    }

    public void UIControl()
    {
        float sound = slider.value;

        if (sound == -40f)
            mixer.SetFloat("UI", -80f);
        else
            mixer.SetFloat("UI", sound);
    }
}
