using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider sliderMaster;
    public Slider sliderBGM;
    public Slider sliderSFX;
    public Slider sliderUI;

    private void Start()
    {
        sliderMaster.onValueChanged.AddListener(delegate { MasterControl(); });
        sliderBGM.onValueChanged.AddListener(delegate { BGMControl(); });
        sliderSFX.onValueChanged.AddListener(delegate { SFXControl(); });
        sliderUI.onValueChanged.AddListener(delegate { UIControl(); });
    }
    public void MasterUp()
    {
        if (sliderMaster.value < 0)
        {
            Debug.Log("vol up");
            sliderMaster.value += 2;
        }
        mixer.SetFloat("Master", sliderMaster.value);
    }
    public void MasterDown()
    {
        if (sliderMaster.value > 40)
        {
            Debug.Log("vol down");
            sliderMaster.value -= 2;
        }
    }

    public void BGMUp()
    {
        if (sliderBGM.value < 0f)
            sliderBGM.value += 2f;
    }
    public void BGMDown()
    {
        if (sliderBGM.value > 40f)
            sliderBGM.value -= 2f;
    }

    public void SFXUp()
    {
        if (sliderSFX.value < 0f)
            sliderSFX.value += 2f;
    }
    public void SFXDown()
    {
        if (sliderSFX.value > 40f)
            sliderSFX.value -= 2f;
    }

    public void UIUp()
    {
        if (sliderUI.value < 0f)
            sliderUI.value += 2f;
    }
    public void UIDown()
    {
        if (sliderUI.value > 40f)
            sliderUI.value -= 2f;
    }
    
    public void MasterControl()
    {
        float sound = sliderMaster.value;

        if (sound == -40f)
            mixer.SetFloat("Master", -80f);
        else
            mixer.SetFloat("Master", sound);
    }

    public void BGMControl()
    {
        float sound = sliderBGM.value;

        if (sound == -40f)
            mixer.SetFloat("BGM", -80f);
        else
            mixer.SetFloat("BGM", sound);
    }

    public void SFXControl()
    {
        float sound = sliderSFX.value;

        if (sound == -40f)
            mixer.SetFloat("SFX", -80f);
        else
            mixer.SetFloat("SFX", sound);
    }

    public void UIControl()
    {
        float sound = sliderUI.value;

        if (sound == -40f)
            mixer.SetFloat("UI", -80f);
        else
            mixer.SetFloat("UI", sound);
    }
}
