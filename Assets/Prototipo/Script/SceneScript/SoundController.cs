using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public AudioSource mixer;
    public Slider slider;

    void Start()
    {
        slider.value = mixer.volume;
    }

    /* public void SetLevel(float sliderValue)
     {
         mixer.volume = sliderValue;
     }*/
    public void SetLevel(float sliderValue)
    {
        mixer.volume = sliderValue;
    }

}