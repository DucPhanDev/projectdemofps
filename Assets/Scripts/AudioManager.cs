using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMono<AudioManager>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip firstAudioClip;
    [SerializeField] private AudioClip echoAudioClip;
    [SerializeField] private AudioClip winAudioClip;

    public void PlayLastWaveAudio()
    {
        audioSource.PlayOneShot(echoAudioClip);
    }

    public void PlayStartWaveAudio()
    {
        audioSource.PlayOneShot(firstAudioClip);
    }

    public void PlayWinAudio()
    {
        audioSource.PlayOneShot(winAudioClip);
    }
}
