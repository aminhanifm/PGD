using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static AudioClip playerjumpsound, playerhitsound, playerdeathsound, playershotsound, powerupsound;
    static AudioSource audioSrc;
    private VolumeManager volmanager;

    void Start()
    {
        playerjumpsound = Resources.Load<AudioClip>("jump");
        playerhitsound = Resources.Load<AudioClip>("hit");
        playerdeathsound = Resources.Load<AudioClip>("death");
        playershotsound = Resources.Load<AudioClip>("gun");
        powerupsound = Resources.Load<AudioClip>("powerup");
        volmanager = FindObjectOfType(typeof(VolumeManager)) as VolumeManager;

        audioSrc = GetComponent<AudioSource> ();
        StartCoroutine(StartFade(audioSrc, 1f, 1f));
    }

    public static void PlaySound (string clip)
    {
        switch (clip) 
        {
            case "jump":
                audioSrc.PlayOneShot(playerjumpsound);
                break;
            case "hit":
                audioSrc.PlayOneShot(playerhitsound);
                break;
            case "death":
                audioSrc.PlayOneShot(playerdeathsound);
                break;
            case "gun":
                audioSrc.PlayOneShot(playershotsound);
                break;
            case "powerup":
                audioSrc.PlayOneShot(powerupsound);
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