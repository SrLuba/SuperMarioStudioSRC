using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioClip SFX;
    public void OnTriggerEnter2D(Collider2D col) {
        if (col.tag!="Player")return;
        SoundManager.instance.Play(29192,SFX,1f,1f);
    }
}
