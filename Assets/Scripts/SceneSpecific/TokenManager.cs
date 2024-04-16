using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenManager : MonoBehaviour {
	public GameObject waitingICON;
	public InputField token, user;

	public AudioClip tryAC, errorAC, successAC;
	IEnumerator tryToken() {
		if (Internet.actualToken != "?")
			yield break;
		this.waitingICON.SetActive (true);
		WWW TRY = new WWW (Internet.mainPage + "token.php?USER=" + user.text + "&TOKEN=" + token.text);
		yield return TRY;
		if(TRY.error!=null) Debug.Log ("ERROR TRYING TO CONNECT OR SOMETHING ");
		while (TRY.text == "")
			yield return new WaitForSeconds (1f);

		waitingICON.SetActive (false);
		if (TRY.text == "ACCESS GRANTED") {
			SoundManager.instance.Play (successAC);
			Debug.Log ("<color=green>TOKEN IS VALID</color>");
			Internet.actualToken = token.text;
			UnityEngine.SceneManagement.SceneManager.LoadScene ("PreInit");
		} else {
			SoundManager.instance.Play (errorAC);
			Debug.Log ("ERROR TRYING TO CONNECT OR SOMETHING | " + TRY.text);
		}

	}

	public void Try() {
		SoundManager.instance.Play (tryAC);
		StartCoroutine (tryToken ());
	}
}
