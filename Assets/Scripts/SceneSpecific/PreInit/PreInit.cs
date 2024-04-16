using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreInit : MonoBehaviour {
	public Transform Eng, Spa;
	public SpriteRenderer engr, spar;
	public Color On,Off;
	public AudioClip musicEnder;
	public GameObject UI, TeamStartOB, PreInitOB;
	bool end;
	int index = 0;
	public AudioClip acceptC, switchC;
	

	void FixedUpdate () {
		if (!end) {
			if (index == 0) {
				engr.color = Color.Lerp (engr.color, On, 15 * Time.deltaTime);
				spar.color = Color.Lerp (spar.color, Off, 15 * Time.deltaTime);
				Eng.transform.position = Vector3.Lerp (Eng.transform.position, new Vector3 (0f, 0f, 0f), 15 * Time.deltaTime);
				Eng.transform.localScale = Vector3.Lerp (Eng.transform.localScale, new Vector3 (5f, 5f, 5f), 15f * Time.deltaTime);
				Spa.transform.position = Vector3.Lerp (Spa.transform.position, new Vector3 (100f, 0f, 0f), 15 * Time.deltaTime);
				Spa.transform.localScale = Vector3.Lerp (Spa.transform.localScale, new Vector3 (2f, 2f, 2f), 15f * Time.deltaTime);
				Lang.lang = "ENG";
			} else if (index == 1) {
				engr.color = Color.Lerp (engr.color, Off, 15 * Time.deltaTime);
				spar.color = Color.Lerp (spar.color, On, 15 * Time.deltaTime);
				Eng.transform.position = Vector3.Lerp (Eng.transform.position, new Vector3 (-100f, 0f, 0f), 15 * Time.deltaTime);
				Eng.transform.localScale = Vector3.Lerp (Eng.transform.localScale, new Vector3 (2f, 2f, 2f), 15f * Time.deltaTime);
				Spa.transform.position = Vector3.Lerp (Spa.transform.position, new Vector3 (0f, 0f, 0f), 15 * Time.deltaTime);
				Spa.transform.localScale = Vector3.Lerp (Spa.transform.localScale, new Vector3 (5f, 5f, 5f), 15f * Time.deltaTime);
				Lang.lang = "SPA";
			}
		} else {
			spar.color = Color.Lerp (spar.color, Off, 15 * Time.deltaTime);
			engr.color = Color.Lerp (engr.color, Off, 15 * Time.deltaTime);
			Eng.transform.position = Vector3.Lerp (Eng.transform.position, new Vector3 (0f, -200f, 0f), 15 * Time.deltaTime);
			Eng.transform.localScale = Vector3.Lerp (Eng.transform.localScale, new Vector3 (1f, 1f, 1f), 15f * Time.deltaTime);
			Spa.transform.position = Vector3.Lerp (Spa.transform.position, new Vector3 (100f, -200f, 0f), 15 * Time.deltaTime);
			Spa.transform.localScale = Vector3.Lerp (Spa.transform.localScale, new Vector3 (1f, 1f, 1f), 15f * Time.deltaTime);
		}
	}
	IEnumerator End() {
		SoundManager.instance.Play (acceptC);
		end = true;
		//LE.Audio.Music.Play (0,musicEnder);
		UI.SetActive (false);
		yield return new WaitForSeconds (3f);
		TeamStartOB.SetActive (true);
		PreInitOB.SetActive (false);
		this.enabled = false;

	}
	void Update() {
		if (GlobalInput.rightp) {
			index++;
			if (index > 1) {
				index = 0;
			} else {
				SoundManager.instance.Play (switchC);
			}
		}
		if (GlobalInput.leftp) {
			index--;
			if (index < 0) {
				index = 1;
			}else {
				SoundManager.instance.Play (switchC);
			}
		}
		if (GlobalInput.ap) {
			StartCoroutine (End());
		}
	}
}
