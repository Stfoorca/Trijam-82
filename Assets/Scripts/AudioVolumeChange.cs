using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioVolumeChange : MonoBehaviour
{
    public AudioSource audio;
    public Slider slider;
    public Slider slider2;
    public bool isBG;
    void Start()
    {
        if (!isBG)
            return;
        audio.volume = 0.3f;
        slider.value = 0.3f;
        slider2.value = 0.3f;
    }
    public void SetVolume(float vol)
    {
        audio.volume = vol;
        slider.value = vol;
        slider2.value = vol;
    }
}
