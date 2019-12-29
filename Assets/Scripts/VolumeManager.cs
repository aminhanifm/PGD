using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public AudioSource audiosrc;
    public Slider sliderMM;
    private float audiovol;
    
    void Start()
    {
        
    }

    void Update()
    {
        audiosrc.volume = audiovol;
    }

    public void setvol(float vol)
    {
        audiovol = vol;
    }
}
