using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public void Awake() {instance=this;}
    public void Play(int dummy, AudioClip clip, float volume, float pitch) {
        GameObject gb = new GameObject(dummy.ToString()+"_SFX_"+clip.name);
        AudioSource src = gb.AddComponent<AudioSource>();
        gb.transform.position = Camera.main.transform.position;
        gb.transform.SetParent(Camera.main.transform);
        src.clip = clip;
        src.volume = volume;
        src.pitch = pitch;
        src.Play();
        gb.AddComponent<DestroyOnSecs>().timer=clip.length;
    }
     public void Play(int dummy, AudioClip clip) {
        GameObject gb = new GameObject(dummy.ToString()+"_SFX_"+clip.name);
        AudioSource src = gb.AddComponent<AudioSource>();
        gb.transform.position = Camera.main.transform.position;
        gb.transform.SetParent(Camera.main.transform);
        src.clip = clip;
        src.Play();
        gb.AddComponent<DestroyOnSecs>().timer=clip.length;
    }
    public void Play(AudioClip clip, float volume, float pitch) {
        GameObject gb = new GameObject("SFX_"+clip.name);
        AudioSource src = gb.AddComponent<AudioSource>();
        gb.transform.position = Camera.main.transform.position;
        gb.transform.SetParent(Camera.main.transform);
        src.clip = clip;
        src.volume = volume;
        src.pitch = pitch;
        src.Play();
        gb.AddComponent<DestroyOnSecs>().timer=clip.length;
    }
    public void Play(AudioClip clip) {
        GameObject gb = new GameObject("SFX_"+clip.name);
        AudioSource src = gb.AddComponent<AudioSource>();
        gb.transform.position = Camera.main.transform.position;
        gb.transform.SetParent(Camera.main.transform);
        src.clip = clip;
        src.Play();
        gb.AddComponent<DestroyOnSecs>().timer=clip.length;
    }
     public void Play(AudioClip clip, bool oneTime) {
        if (!oneTime) return;
        if (GameObject.Find("SFX_"+clip.name)!=null)return;
        GameObject gb = new GameObject("SFX_"+clip.name);
        AudioSource src = gb.AddComponent<AudioSource>();
        gb.transform.position = Camera.main.transform.position;
        gb.transform.SetParent(Camera.main.transform);
        src.clip = clip;
        src.Play();
        gb.AddComponent<DestroyOnSecs>().timer=clip.length;
    }
}
