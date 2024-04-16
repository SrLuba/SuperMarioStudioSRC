using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;
	
	public bool play;

    

    public AudioSource musicNormal, musicYoshi;
    public AudioLowPassFilter filter1;
    public AudioReverbFilter filter2;

    public float frequencyNormal, frequencyUnderWater;
    public float reverbNormal, reverbUnderwater;
    public float volumeNormal, volumeUnderwater;

    public float changeSpeed;

    public MusicData defaultmusicdata;
    public MusicDataClass defaultMusic;

    public bool underWater;
    public bool Yoshi;

    bool inTransition = false;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Global.Game.SaveData.musicData!=null) SwitchMusic(Global.Game.SaveData.musicData);

        defaultMusic = defaultmusicdata.data;
        musicNormal.loop = true;
        musicNormal.Play();
        musicYoshi.volume = 0f;
        musicYoshi.loop = true;
        musicYoshi.Play();
    }
    public void SwitchMusicPersistent(MusicDataClass data) {
        defaultMusic=data;
        SwitchMusic(data);
    }

	public void SwitchMusic(MusicData data){
		musicYoshi.clip = data.data.Ym;
		musicNormal.clip = data.data.Nm;
		musicNormal.loop = true;
        musicYoshi.loop = true;
        musicNormal.Play();
        musicYoshi.Play();
	}
    public void SwitchMusic(MusicDataClass data){
		musicYoshi.clip = data.Ym;
		musicNormal.clip = data.Nm;
		musicNormal.loop = true;
        musicYoshi.loop = true;
        musicNormal.Play();
        musicYoshi.Play();
	}
	public void RequestHaltMusicAndPlay(AudioClip clip) {
		StartCoroutine(HaltMusicRaw(clip.length+0.5f));
		musicNormal.clip = clip;
        musicNormal.loop = false;
        musicNormal.Play();
	}
    public void RequestHaltMusicAndPlay(float time, AudioClip clip) {
        StartCoroutine(HaltMusicRaw(time));
        musicNormal.clip = clip;
        musicNormal.loop = false;
        musicNormal.Play();
    }
     public AudioClip reverseSnare;
	public void RequestTransition(MusicDataClass stream, AudioClip transitionClip, bool persistent){
        SwitchMusic(stream);
	}
    public void RequestTransition(MusicData stream, AudioClip transitionClip, bool persistent){
        SwitchMusic(stream);
	}
   
    public void RequestTransitionDefault() {
        inTransition=false;
        StartCoroutine(Transition(this.defaultMusic, reverseSnare,true));
	}
	public IEnumerator Transition(MusicDataClass stream, AudioClip transitionClip, bool persistent){
        if (!inTransition) {
            inTransition=true;
    		SoundManager.instance.Play(99,transitionClip,1f,1f);
            play=false;
            Yoshi=false;
            while(musicNormal.volume>0.01f){
                musicNormal.volume = Mathf.Lerp(musicNormal.volume,0f,5f*Time.deltaTime);
                yield return new WaitForSeconds(0.001f);
            }
            musicNormal.volume=0f;
    		yield return new WaitForSeconds(transitionClip.length);
            if (persistent) defaultMusic=stream;
            SwitchMusic(stream);
            musicNormal.volume=0f;
            Yoshi=false;
            while(musicNormal.volume<0.99f){
                musicNormal.volume = Mathf.Lerp(musicNormal.volume,1f,5f*Time.deltaTime);
                yield return new WaitForSeconds(0.001f);
            }
            musicNormal.volume = 1f;      
            musicYoshi.volume = 1f;
            play=true;
            inTransition=false;
        }
	}
	public void StopMusic() {
		musicNormal.Stop();
		musicYoshi.Stop();
	}
	public void HaltMusicRequest(float time) {StartCoroutine(HaltMusic(time));}
	public IEnumerator HaltMusic(float time){
        if (!inTransition) {
            inTransition=true;
            play=false;
    		while(musicNormal.volume>0f){
                musicNormal.volume-=Time.deltaTime;
                yield return new WaitForSeconds(0.001f);
            }
    		
    		yield return new WaitForSeconds(time);
    		
    		while(musicNormal.volume>1f){
                musicNormal.volume+=Time.deltaTime;
                yield return new WaitForSeconds(0.001f);
            }

            musicNormal.volume=1f;
            inTransition=false;
        }
	}
    public IEnumerator HaltMusicRaw(float time){

		yield return new WaitForSeconds(time);
        RequestTransitionDefault();
	}
    public void FixedUpdate()
    {
        if (play) {
            musicNormal.volume = Mathf.Lerp(musicNormal.volume, (Yoshi) ? 0f : (underWater ? volumeUnderwater : volumeNormal), changeSpeed * Time.deltaTime);
            musicYoshi.volume = Mathf.Lerp(musicYoshi.volume, (!Yoshi) ? 0f : (underWater ? volumeUnderwater : volumeNormal), changeSpeed * Time.deltaTime);
            filter1.cutoffFrequency = Mathf.Lerp(filter1.cutoffFrequency, underWater ? frequencyUnderWater : frequencyNormal, changeSpeed * Time.deltaTime);
            filter2.reverbLevel = Mathf.Lerp(filter2.reverbLevel, underWater ? reverbUnderwater : reverbNormal, changeSpeed * Time.deltaTime);
        }
    }
}
