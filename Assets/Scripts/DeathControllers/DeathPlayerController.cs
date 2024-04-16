using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlayerController : MonoBehaviour
{
	public GameObject WhiteFade;
	public float haltTime =1f;
	public AudioClip initialClip, deathMusic;
	public bool musicAfterHalt;
	public float initialVY;
	public float gravity;
	public float rotateSPD;
	float timer = 0f;

	public bool musicFix = true;
	
	float vy = 0f;
	
	bool halt = true;
   
    void Start()
    {
		vy = 0f;
        timer = 0f;
		if (initialClip != null) SoundManager.instance.Play(1, initialClip, 1f, 1f);
		if (!musicAfterHalt) {
			SoundManager.instance.Play(73467, deathMusic, 1f, 1f);
		}
		//if(!musicFix)MusicController.instance.StopMusic();
    }
	
	bool halt2=true;

    float timerReset = 5f;
    void FixedUpdate()
    {
        timer+=Time.deltaTime;
		if (!halt) {
			this.transform.Translate(0f,vy*Time.deltaTime,0f);
			vy-=gravity*Time.deltaTime;
			//this.transform.Rotate(0f,0f,rotateSPD*Time.deltaTime);
		}else {
			if (timer > haltTime) {
				if (musicAfterHalt) SoundManager.instance.Play(73467, deathMusic, 1f, 1f);
    	        if(halt2 && WhiteFade!=null) Instantiate(WhiteFade, Camera.main.gameObject.transform.position, Quaternion.identity).transform.SetParent(Camera.main.gameObject.transform);

				vy = initialVY;
				halt = false;
				halt2=false;
			}
		}

		timerReset-=Time.deltaTime;
		if (timerReset<0f) Global.Game.ResetLevel();
    }
}
