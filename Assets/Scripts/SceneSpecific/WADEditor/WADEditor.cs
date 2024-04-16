using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WADEditor : MonoBehaviour {
	public Image SpriteDisplay;
	public Text text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Sprite sprite = LE_Res.getSprite (WADManager.wad.lumps [WADManager.instance.idSelect].lumpName, WADManager.wad);
		SpriteDisplay.sprite = sprite;
		SpriteDisplay.SetNativeSize ();
		SpriteDisplay.transform.localScale = new Vector3 (0.03f, 0.03f, 0.03f);

		text.text = WADManager.wad.lumps [WADManager.instance.idSelect].lumpName;
	}
}
