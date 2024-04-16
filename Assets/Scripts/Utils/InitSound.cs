using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSound : MonoBehaviour
{
    public AudioClip clip;
    public int channel;
    public float volume, pitch;
    void Start()
    {
        SoundManager.instance.Play(channel, clip, volume, pitch);
    }
}
