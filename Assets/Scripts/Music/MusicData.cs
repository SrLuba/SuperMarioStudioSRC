using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]public class MusicDataClass {

    public AudioClip Nm, Ym;
    public MusicDataClass(AudioClip clip) {
        this.Nm=clip;
        this.Ym=clip;
    }
    public MusicDataClass(AudioClip clipA, AudioClip clipB)  {
        this.Nm=clipA;
        this.Ym=clipB;
    }
}
[CreateAssetMenu]public class MusicData : ScriptableObject
{
    public MusicDataClass data;
}
