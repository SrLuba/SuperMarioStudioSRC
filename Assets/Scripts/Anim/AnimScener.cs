using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScener : MonoBehaviour {

	public string nameper;
	public void LoadScene(string name) {
		if (GameObject.Find ("Music_Channel_0")) {
			DontDestroyOnLoad (GameObject.Find ("Music_Channel_0"));
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene (name);
	}
	public void LoadSceneLoader()
	{
		Global.LoadScene(nameper);
	}
}
