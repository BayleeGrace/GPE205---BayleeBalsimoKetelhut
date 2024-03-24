using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SliderChangeForVolume : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void OnSliderChange()
    {
        audioMixer.SetFloat("mainVolume", 20.0f); // Set the volume to 20db
    }
}
