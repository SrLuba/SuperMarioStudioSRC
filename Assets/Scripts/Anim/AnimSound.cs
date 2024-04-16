using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSound : MonoBehaviour {
	public List<AudioClip> CLIPS;
	public void Play(int id) {
		SoundManager.instance.Play (CLIPS [id]);
	}
}
