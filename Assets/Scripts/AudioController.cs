using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    public AudioSource aus;
    public AudioClip gunSound;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySound(AudioClip sound)
    {
        if(aus != null && sound)
        {
            aus.PlayOneShot(sound);
        }
    }
}
