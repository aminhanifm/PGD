using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static AudioClip button, carhit, wanted;
    static AudioSource audioSrc;
    private VolumeManager volmanager;

    void Start()
    {
        button = Resources.Load<AudioClip>("Button");
        carhit = Resources.Load<AudioClip>("Car Hit");
        wanted = Resources.Load<AudioClip>("Wanted");

        volmanager = FindObjectOfType(typeof(VolumeManager)) as VolumeManager;

        audioSrc = GetComponent<AudioSource> ();
        StartCoroutine(StartFade(audioSrc, 1f, 1f));
    }

    public static void PlaySound (string clip)
    {
        switch (clip) 
        {
            case "Button":
                audioSrc.PlayOneShot(button);
                break;
            case "Car Hit":
                audioSrc.PlayOneShot(carhit);
                break;
            case "Wanted":
                audioSrc.PlayOneShot(wanted);
                break;
        }
    }

    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            volmanager.sliderMM.value = audioSource.volume;
            yield return null;
        }

        yield break;
    }
}